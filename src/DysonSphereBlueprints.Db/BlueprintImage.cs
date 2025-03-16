namespace DysonSphereBlueprints.Db;

public class BlueprintImage
{
    public string ImageId { get; set; }
    public int BlueprintId { get; set; }
    public byte[] Image { get; set; }

    public Blueprint Blueprint { get; set; }
}