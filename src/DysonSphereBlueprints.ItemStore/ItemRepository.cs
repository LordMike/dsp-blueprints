using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using DysonSphereBlueprints.ItemStore.Model;

namespace DysonSphereBlueprints.ItemStore;

public class ItemRepository
{
    public static ItemRepository Instance { get; } = new();

    public IReadOnlyDictionary<int, PowerUsageItem> PowerUsages { get; }
    public IReadOnlyDictionary<int, ItemProtoSetItem> Items { get; }
    public IReadOnlyDictionary<int, AchievementProtoSetItem> Achievements { get; }
    public IReadOnlyDictionary<int, RecipeProtoSetItem> Recipes { get; }
    public IReadOnlyDictionary<int, TechProtoSetItem> Techs { get; }
    public IReadOnlyDictionary<int, ThemeProtoSetItem> Themes { get; }
    public IReadOnlyDictionary<int, VeinProtoSetItem> Veins { get; }

    private ItemRepository()
    {
        JsonSerializerOptions serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
        
        using (Stream? fs =
               typeof(ItemRepository).Assembly.GetManifestResourceStream(
                   "DysonSphereBlueprints.ItemStore.Resources.powerusage.json"))
        {
            PowerUsageItem[] store = JsonSerializer.Deserialize<PowerUsageItem[]>(fs, serializerOptions)!;

            PowerUsages = store.ToDictionary(s => s.Id);
        }

        using (Stream? fs =
               typeof(ItemRepository).Assembly.GetManifestResourceStream(
                   "DysonSphereBlueprints.ItemStore.Resources.protosets.json"))
        {
            RootObject store = JsonSerializer.Deserialize<RootObject>(fs, serializerOptions)!;

            Items = store.ItemProtoSet.dataArray.ToDictionary(s => s.ID);
            Achievements = store.AchievementProtoSet.dataArray.ToDictionary(s => s.ID);
            Recipes = store.RecipeProtoSet.dataArray.ToDictionary(s => s.ID);
            Techs = store.TechProtoSet.dataArray.ToDictionary(s => s.ID);
            Themes = store.ThemeProtoSet.dataArray.ToDictionary(s => s.ID);
            Veins = store.VeinProtoSet.dataArray.ToDictionary(s => s.ID);
        }
    }
}