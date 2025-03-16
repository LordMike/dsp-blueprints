namespace DysonSphereBlueprints.ItemStore.Model;

public class TechProtoSet
{
    public string TableName { get; init; }
    public string Signature { get; init; }
    public TechProtoSetItem[] dataArray { get; init; }
}