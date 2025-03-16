using DysonSphereBlueprints.Analysis.Analysis;
using DysonSphereBlueprints.Analysis.Enums;
using DysonSphereBlueprints.Web.Code.Model;
using Radzen;

namespace DysonSphereBlueprints.Web.Code.Modifiers;

public class TweakAddWarpers(BlueprintEditModel bpModel, NotificationService notificationService)
    : BlueprintAction<BlueprintInterstellarLogisticsStationModel>(notificationService)
{
    public override string Title => "Add 100 warpers";
    public override string Description => "Add 100 warpers with local demand/remote storage to all ILS, in empty spots";

    protected override IEnumerable<BlueprintInterstellarLogisticsStationModel> DiscoverEntries()
    {
        return bpModel.InterstellarLogisticsStations;
    }

    public override bool CanApply(BlueprintInterstellarLogisticsStationModel entry)
    {
        return entry.StorageSlots.Any(s => s.Item == DspItem.SpaceWarper && s.Max != 100 && s.LocalLogic != LogisticRole.Demand && s.RemoteLogic != LogisticRole.None) || entry.StorageSlots.Any(s => !s.HasItem);
    }

    protected override BlueprintActionResult PerformSingle(BlueprintInterstellarLogisticsStationModel entry)
    {
        
        
        BlueprintLogisticsStationStorageModel? existingEntry =
            entry.StorageSlots.FirstOrDefault(s => s.Item == DspItem.SpaceWarper) ??
            entry.StorageSlots.LastOrDefault(s => !s.HasItem);

        if (existingEntry != null)
        {
            if (existingEntry is
                {
                    Item: DspItem.SpaceWarper, LocalLogic: LogisticRole.Demand, RemoteLogic: LogisticRole.None, Max: 100
                })
                return BlueprintActionResult.Skipped; // Already set
            existingEntry.Item = DspItem.SpaceWarper;
            existingEntry.LocalLogic = LogisticRole.Demand;
            existingEntry.RemoteLogic = LogisticRole.None;
            existingEntry.Max = 100;
            return BlueprintActionResult.Success; // Altered
        }

        return BlueprintActionResult.Failed; // Not possible
    }
}