namespace DysonSphereBlueprints.Db;

public class Blueprint
{
    public int Id { get; set; }
    public string Url { get; set; }
    public string UrlId { get; set; }
    public string Author { get; set; }
    public string AuthorId { get; set; }
    public string? Collection { get; set; }
    public string? CollectionId { get; set; }
    public string GameVersion { get; set; }
    public int Copied { get; set; }
    public int Favorited { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public string BlueprintString { get; set; }
    public string Tags { get; set; }

    public IList<BlueprintImage> Images { get; set; }
}