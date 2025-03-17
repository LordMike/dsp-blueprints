using System.IO.Compression;
using System.Reflection;
using System.Text;

namespace DysonSphereBlueprints.Web.Code;

public record BlueprintSample(string Name, string Path, bool UsesGz);

public static class BlueprintsSamples
{
    private static BlueprintSample[] _samples =
    [
        new("Full planet Carrier Rockets", "Resources/Samples/Full planet Carrier Rockets.txt.gz", true),
        new("Grabby Hands FFF-05B", "Resources/Samples/Grabby Hands FFF-05B.txt.gz", true),
        new("Dense01", "Resources/Samples/DENSE Energetic Graphite 14,4K pmin.txt.gz", true),
    ];

    public static IEnumerable<BlueprintSample> GetSamples() => _samples;

    public static async Task<string> GetBlueprintSampleContent(HttpClient http, BlueprintSample sample)
    {
        await using Stream res = await http.GetStreamAsync(sample.Path);

        Stream decompStream = res;
        if (sample.UsesGz)
            decompStream = new GZipStream(decompStream, CompressionMode.Decompress);

        await using (decompStream)
        {
            using StreamReader sr = new StreamReader(decompStream);
            return await sr.ReadToEndAsync();
        }
    }
}