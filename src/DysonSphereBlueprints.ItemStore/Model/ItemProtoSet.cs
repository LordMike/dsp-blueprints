namespace DysonSphereBlueprints.ItemStore.Model;

public class ItemProtoSet
{
    public string TableName { get; init; }
    public string Signature { get; init; }
    public ItemProtoSetItem[] dataArray { get; init; }
}