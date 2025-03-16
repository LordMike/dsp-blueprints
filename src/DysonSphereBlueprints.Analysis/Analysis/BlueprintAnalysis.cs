using DysonSphereBlueprints.Analysis.Enums;
using DysonSphereBlueprints.ItemStore.Model;

namespace DysonSphereBlueprints.Analysis.Analysis;

public class BlueprintAnalysis
{
    public required Dictionary<DspRecipe, int> RecipeCounts { get; init; }
    public required Dictionary<DspItem, int> BuildingCounts { get; init; }
    public required Dictionary<DspItem, ItemLogisticsInfo> ItemLogisticsInfos { get; init; }

    public int BuildingCount => BuildingCounts.Sum(s => s.Value);
    public PowerUsageRange PowerUsage { get; init; }
    public PowerUsageRange PowerProvides { get; init; }
}