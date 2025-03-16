namespace DysonSphereBlueprints.ItemStore.Model;

public class ThemeProtoSet
{
    public string TableName { get; init; }
    public string Signature { get; init; }
    public ThemeProtoSetItem[] dataArray { get; init; }
}