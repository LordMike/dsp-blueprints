using DysonSphereBlueprints.ItemStore.Enums;

namespace DysonSphereBlueprints.Viewer.Extensions;

static class Queries
{
    public static IEnumerable<BlueprintAnalysisPair> Has(this IEnumerable<BlueprintAnalysisPair> source, DspItem building) =>
        source.Where(s => s.Analysis.BuildingCounts.ContainsKey(building));

    public static IEnumerable<BlueprintAnalysisPair> Produces(this IEnumerable<BlueprintAnalysisPair> source,
        params DspRecipe[] recipes) =>
        source.Where(s => recipes.All(r => s.Analysis.RecipeCounts.ContainsKey(r)));

    public static IEnumerable<BlueprintAnalysisPair> ProducesOneOf(this IEnumerable<BlueprintAnalysisPair> source,
        params DspRecipe[] recipes) =>
        source.Where(s => recipes.Any(r => s.Analysis.RecipeCounts.ContainsKey(r)));

    public static IEnumerable<BlueprintAnalysisPair> MaxBuildingCount(this IEnumerable<BlueprintAnalysisPair> source,
        int maxCount) =>
        source.Where(s => s.Analysis.BuildingCount <= maxCount);
}