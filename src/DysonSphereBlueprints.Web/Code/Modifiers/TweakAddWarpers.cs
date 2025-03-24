using DysonSphereBlueprints.Analysis.Analysis;
using DysonSphereBlueprints.ItemStore.Enums;
using DysonSphereBlueprints.Web.Code.Model;
using Radzen;

namespace DysonSphereBlueprints.Web.Code.Modifiers;

public class TweakAddWarpers(NotificationService notificationService)
    : BlueprintAction<BlueprintInterstellarLogisticsStationModel>(notificationService)
{
    public override string Title => "Add 100 warpers";
    public override string Description => "Add 100 warpers with local demand/remote storage to all ILS, in empty spots";

    protected override IEnumerable<BlueprintInterstellarLogisticsStationModel> DiscoverEntries(BlueprintEditModel bpModel)
    {
        return bpModel.InterstellarLogisticsStations;
    }

    public override bool CanApply(BlueprintInterstellarLogisticsStationModel entry)
    {
        BlueprintLogisticsStationStorageModel? storageSlot =
            entry.Storages.FirstOrDefault(s => s.Item == DspItem.SpaceWarper) ??
            entry.Storages.LastOrDefault(s => !s.HasItem);

        if (storageSlot == null)
            return false;

        if (storageSlot is
            {
                Item: DspItem.SpaceWarper, LocalLogic: LogisticRole.Demand, RemoteLogic: LogisticRole.None, Max: 100
            })
            return false;

        return true;
    }

    protected override void PerformSingle(BlueprintInterstellarLogisticsStationModel entry)
    {
        BlueprintLogisticsStationStorageModel? existingEntry =
            entry.Storages.FirstOrDefault(s => s.Item == DspItem.SpaceWarper) ??
            entry.Storages.LastOrDefault(s => !s.HasItem);

        if (existingEntry == null)
            throw new InvalidOperationException("Unexpected");

        existingEntry.Item = DspItem.SpaceWarper;
        existingEntry.LocalLogic = LogisticRole.Demand;
        existingEntry.RemoteLogic = LogisticRole.None;
        existingEntry.Max = 100;
    }
}