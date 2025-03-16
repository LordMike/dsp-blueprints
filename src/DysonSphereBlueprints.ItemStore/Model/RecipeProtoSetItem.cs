namespace DysonSphereBlueprints.ItemStore.Model;

public class RecipeProtoSetItem
{
    public string Name { get; init; }
    public int ID { get; init; }
    public string SID { get; init; }
    public string Type { get; init; }
    public bool Handcraft { get; init; }
    public bool Explicit { get; init; }
    public int TimeSpend { get; init; }
    public int[] Items { get; init; }
    public int[] ItemCounts { get; init; }
    public int[] Results { get; init; }
    public int[] ResultCounts { get; init; }
    public int GridIndex { get; init; }
    public string IconPath { get; init; }
    public string Description { get; init; }
    public bool NonProductive { get; init; }
}