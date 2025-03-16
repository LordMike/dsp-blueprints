using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using DysonSphereBlueprints.Db;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DysonSphereBlueprints.Ripper;

class Runner(
    IHttpClientFactory httpClientFactory,
    ILogger<Runner> logger,
    BlueprintContext blueprintContext)
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("polly");

    private readonly Regex _rgxPageLink = new(@"^/blueprints\?page=(?<page>[0-9]+)",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private readonly Regex _rgxAuthorLinkId = new("^/users/(?<id>[0-9]+)/blueprints$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private readonly Regex _rgxCollectionLinkId =
        new(@"^/collections/(?<id>[^/]+)$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

    public async Task Run(CancellationToken token)
    {
        HashSet<int> existingIds = await blueprintContext.Blueprints.Select(s => s.Id).ToHashSetAsync(token);

        logger.LogInformation("Loaded {Count} existing blueprint ids", existingIds.Count);

        await foreach (Uri pageUri in GetPages(token))
        {
            List<BluePrintLink> links = await GetBlueprintLinks(pageUri, token).ToListAsync(token);

            logger.LogInformation("Fetched {Count} links", links.Count);

            List<BluePrintLink> toFetch = links.ExceptBy(existingIds, s => s.Id).ToList();

            logger.LogInformation("Downloading {Count} new links", toFetch.Count);

            if (toFetch.Count == 0)
                continue;

            foreach (BluePrintLink bluePrintLink in toFetch)
            {
                Blueprint newBlueprint = await FetchBlueprint(bluePrintLink, token);
                blueprintContext.Blueprints.Add(newBlueprint);
            }

            await blueprintContext.SaveChangesAsync(token);
        }
    }

    private async Task<HttpResponseMessage> GetResponse(Uri uri, bool checkResponseCode, CancellationToken token)
    {
        Stopwatch sw = Stopwatch.StartNew();
        HttpResponseMessage resp = await _httpClient.GetAsync(uri, HttpCompletionOption.ResponseContentRead, token);
        sw.Stop();

        logger.LogDebug("Fetched {Url}, {StatusCode} in {Elapsed}", uri, resp.StatusCode, sw.Elapsed);

        if (checkResponseCode)
            resp.EnsureSuccessStatusCode();

        return resp;
    }

    private async Task<HtmlDocument> GetHtml(Uri uri, CancellationToken token)
    {
        using HttpResponseMessage resp = await GetResponse(uri, true, token);
        string contentStr = await resp.Content.ReadAsStringAsync(token);

        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(contentStr);

        return doc;
    }

    private async IAsyncEnumerable<Uri> GetPages(CancellationToken token)
    {
        HtmlDocument html =
            await GetHtml(new Uri("https://www.dysonsphereblueprints.com/blueprints?type=factory"), token);
        HtmlNode? node = html.DocumentNode.SelectSingleNode("//nav/span[@class='last']/a");

        string href = HttpUtility.HtmlDecode(node.GetAttributeValue("href", null));
        int pageNo = int.Parse(_rgxPageLink.Match(href).Groups["page"].Value);

        for (int i = 1; i <= pageNo; i++)
            yield return new Uri("https://www.dysonsphereblueprints.com/blueprints?type=factory&page=" + i);
    }

    private async IAsyncEnumerable<BluePrintLink> GetBlueprintLinks(Uri uri, CancellationToken token)
    {
        HtmlDocument html = await GetHtml(uri, token);
        HtmlNodeCollection? nodes = html.DocumentNode.SelectNodes(
            "//div[@class='t-blueprint-list']//li[@class='o-blueprint-card factory']");

        foreach (HtmlNode? node in nodes)
        {
            int id = node.GetAttributeValue("data-blueprint-id", -1);
            HtmlNode? linkNode = node.SelectSingleNode("./div/a");

            string? href = linkNode.GetAttributeValue("href", null);
            Uri blueprintUri = new Uri(uri, href);

            string urlId = blueprintUri.Segments.Last();

            yield return new BluePrintLink(id, urlId, blueprintUri);
        }
    }

    private async Task<Blueprint> FetchBlueprint(BluePrintLink bluePrintLink, CancellationToken token)
    {
        HtmlDocument html = await GetHtml(bluePrintLink.Url, token);

        HtmlNode? titleNode = html.DocumentNode.SelectSingleNode("//div[@class='t-blueprint__title']/h2");
        string title = HttpUtility.HtmlDecode(titleNode.InnerText.Trim());

        HtmlNode? descriptionContainerNode =
            html.DocumentNode.SelectSingleNode("//div[@class='t-blueprint__description']");
        HtmlNode? descriptionNode = descriptionContainerNode.SelectSingleNode("./div");

        string? description;
        if (descriptionNode != null)
            description = HttpUtility.HtmlDecode(descriptionNode.InnerHtml.Trim());
        else
            description = null;


        HtmlNode? infoNode = html.DocumentNode.SelectSingleNode("//div[@class='t-blueprint__info']");
        HtmlNode? authorNode = infoNode.SelectSingleNode("./ul/li[1]/a");
        string? authorLink = authorNode.GetAttributeValue("href", null);
        string authorId = _rgxAuthorLinkId.Match(authorLink).Groups["id"].Value;
        string authorName = HttpUtility.HtmlDecode(authorNode.InnerText.Trim());

        HtmlNode? collectionNode = infoNode.SelectSingleNode("./ul/li[2]/a");
        string? collectionLink = collectionNode.GetAttributeValue("href", null);
        string collectionId = _rgxCollectionLinkId.Match(collectionLink).Groups["id"].Value;
        string collectionName = HttpUtility.HtmlDecode(collectionNode.InnerText.Trim());

        HtmlNode? gameVersionNode = infoNode.SelectSingleNode("./ul/li[3]/span");
        string gameVersionName = HttpUtility.HtmlDecode(gameVersionNode.InnerText.Trim());

        HtmlNode? copiedNode = infoNode.SelectSingleNode(".//span[@class='t-blueprint__info-usage']");
        int copied = int.Parse(copiedNode.InnerText.Split(' ').First());

        HtmlNode? favoriteNode =
            infoNode.SelectSingleNode(".//div[@class='t-blueprint__info-votes']/strong[@class='count']");
        int favorite = int.Parse(favoriteNode.InnerText.Split(' ').First());

        HtmlNodeCollection? tagsNodes =
            html.DocumentNode.SelectNodes(
                "//div[@class='t-blueprint__tags']/ul/li[@class='t-blueprint__tags-tag']/span");
        string[] tags = tagsNodes
            .Select(s => HttpUtility.HtmlDecode(s.GetAttributeValue("data-tippy-content", null).Trim())).ToArray();

        HtmlNode? blueprintNode = html.DocumentNode.SelectSingleNode("//div[@class='t-blueprint__blueprint']/textarea");
        HtmlNode? blueprintLinkNode = html.DocumentNode.SelectSingleNode("//a[@class='button t-blueprint__open']");

        string blueprintText;
        if (blueprintLinkNode != null)
        {
            // Download
            Uri blueprintLink = new Uri(bluePrintLink.Url, blueprintLinkNode.GetAttributeValue("href", null));
            using HttpResponseMessage blueprintResp = await GetResponse(blueprintLink, true, token);

            blueprintText = await blueprintResp.Content.ReadAsStringAsync(token);
        }
        else if (blueprintNode != null)
        {
            blueprintText = HttpUtility.HtmlDecode(blueprintNode.InnerText.Trim());
        }
        else
            throw new InvalidOperationException();

        HtmlNodeCollection? imageNodes = html.DocumentNode.SelectNodes("//div[@class='swiper-slide']/a[@href]");
        Uri[] imageLinks = imageNodes.Select(s => new Uri(bluePrintLink.Url, s.GetAttributeValue("href", null)))
            .ToArray();

        BlueprintImage[] images = new BlueprintImage[imageLinks.Length];
        for (int i = 0; i < imageLinks.Length; i++)
        {
            using HttpResponseMessage resp = await GetResponse(imageLinks[i], false, token);

            if (resp.StatusCode == HttpStatusCode.BadRequest)
            {
                // Ignore
                continue;
            }

            resp.EnsureSuccessStatusCode();
            byte[] bytes = await resp.Content.ReadAsByteArrayAsync(token);

            images[i] = new BlueprintImage
            {
                ImageId = imageLinks[i].Segments.Last(),
                Image = bytes
            };
        }

        if (images.Any(s => s == null!))
            images = images.Where(s => s != null!).ToArray();

        return new Blueprint
        {
            Id = bluePrintLink.Id,
            Url = bluePrintLink.Url.ToString(),
            UrlId = bluePrintLink.UrlId,
            Author = authorName,
            AuthorId = authorId,
            Collection = collectionName,
            CollectionId = collectionId,
            GameVersion = gameVersionName,
            Copied = copied,
            Favorited = favorite,
            Title = title,
            Description = description,
            BlueprintString = blueprintText,
            Tags = string.Join(";", tags),
            Images = images
        };
    }

    private record BluePrintLink(int Id, string UrlId, Uri Url);
}