namespace DysonSphereBlueprints.ItemStore.Model;

public class RecipeProtoSet
{
    public string TableName { get; init; }
    public string Signature { get; init; }
    public RecipeProtoSetItem[] dataArray { get; init; }
}