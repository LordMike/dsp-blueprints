using System.Runtime.InteropServices;
using DysonSphereBlueprints.Analysis.Analysis;
using DysonSphereBlueprints.ItemStore;
using DysonSphereBlueprints.ItemStore.Enums;
using DysonSphereBlueprints.ItemStore.Model;

namespace DysonSphereBlueprints.Analysis;

public static class BlueprintAnalyzer
{
    public static BlueprintAnalysis AnalyzeBlueprint(BlueprintData blueprint)
    {
        Dictionary<DspRecipe, int> recipeCounts = new Dictionary<DspRecipe, int>();
        Dictionary<DspItem, int> buildingCounts = new Dictionary<DspItem, int>();
        Dictionary<DspItem, ItemLogisticsInfo> itemLogisticsInfos = new Dictionary<DspItem, ItemLogisticsInfo>();

        foreach (BlueprintBuilding building in blueprint.buildings)
        {
            DspItem itemKind = (DspItem)building.itemId;
            DspRecipe recipe = (DspRecipe)building.recipeId;

            if (recipe > 0)
            {
                ref int entry =
                    ref CollectionsMarshal.GetValueRefOrAddDefault(recipeCounts, recipe, out _);
                entry++;
            }

            {
                ref int entry =
                    ref CollectionsMarshal.GetValueRefOrAddDefault(buildingCounts, itemKind, out _);
                entry++;
            }

            if (itemKind is DspItem.PlanetaryLogisticsStation or DspItem.InterstellarLogisticsStation)
            {
                StationInfo station = StationInfo.FromParameters(itemKind, building.parameters);

                for (int i = 0; i < station.Storage.Length; i++)
                {
                    StationStorageItem storageItem = station.Storage[i];
                    if (storageItem.ItemId == 0)
                        continue;

                    ref ItemLogisticsInfo entry =
                        ref CollectionsMarshal.GetValueRefOrAddDefault(itemLogisticsInfos, itemKind,
                            out _);

                    bool localSupply = storageItem.LocalLogic == LogisticRole.Supply;
                    bool localDemand = storageItem.LocalLogic == LogisticRole.Demand;
                    bool remoteSupply = storageItem.RemoteLogic == LogisticRole.Supply;
                    bool remoteDemand = storageItem.RemoteLogic == LogisticRole.Demand;

                    entry = new ItemLogisticsInfo(storageItem.ItemId,
                        localSupply || entry.LocalSupply,
                        localDemand || entry.LocalDemand,
                        remoteSupply || entry.RemoteSupply,
                        remoteDemand || entry.RemoteDemand,
                        entry.TotalMaxStorage + storageItem.Max);
                }
            }
        }

        PowerUsageRange powerUsage = new PowerUsageRange();
        PowerUsageRange powerProvides = new PowerUsageRange();

        foreach ((DspItem key, int count) in buildingCounts)
        {
            if (!ItemRepository.Instance.PowerUsages.TryGetValue((int)key, out PowerUsageItem? usage))
                continue;

            powerUsage += usage.Uses * count;
            powerProvides += usage.Provides * count;
        }

        return new BlueprintAnalysis
        {
            RecipeCounts = recipeCounts,
            BuildingCounts = buildingCounts,
            ItemLogisticsInfos = itemLogisticsInfos,
            PowerUsage = powerUsage,
            PowerProvides = powerProvides
        };
    }
}