namespace DysonSphereBlueprints.ItemStore.Model;

public class VeinProtoSet
{
    public string TableName { get; init; }
    public string Signature { get; init; }
    public VeinProtoSetItem[] dataArray { get; init; }
}