namespace DysonSphereBlueprints.ItemStore.Model;

public class TechProtoSetItem
{
    public string Name { get; init; }
    public int ID { get; init; }
    public string SID { get; init; }
    public string Desc { get; init; }
    public string Conclusion { get; init; }
    public bool Published { get; init; }
    public bool IsHiddenTech { get; init; }
    public bool IsObsolete { get; init; }
    public int[] PreItem { get; init; }
    public int Level { get; init; }
    public int MaxLevel { get; init; }
    public int LevelCoef1 { get; init; }
    public int LevelCoef2 { get; init; }
    public string IconPath { get; init; }
    public bool IsLabTech { get; init; }
    public int[] PreTechs { get; init; }
    public int[] PreTechsImplicit { get; init; }
    public bool PreTechsMax { get; init; }
    public int[] Items { get; init; }
    public int[] ItemPoints { get; init; }
    public int[] PropertyOverrideItems { get; init; }
    public int[] PropertyItemCounts { get; init; }
    public int HashNeeded { get; init; }
    public int[] UnlockRecipes { get; init; }
    public int[] UnlockFunctions { get; init; }
    public double[] UnlockValues { get; init; }
    public int[] AddItems { get; init; }
    public int[] AddItemCounts { get; init; }
    public Position Position { get; init; }
}