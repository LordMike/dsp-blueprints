namespace DysonSphereBlueprints.ItemStore.Model;

public class RootObject
{
    public string version { get;  set; }
    public AchievementProtoSet AchievementProtoSet { get; init; }
    public ItemProtoSet ItemProtoSet { get; init; }
    public RecipeProtoSet RecipeProtoSet { get; init; }
    public TechProtoSet TechProtoSet { get; init; }
    public ThemeProtoSet ThemeProtoSet { get; init; }
    public VeinProtoSet VeinProtoSet { get; init; }
}