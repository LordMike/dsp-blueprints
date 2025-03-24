using DysonSphereBlueprints.Analysis.Analysis;
using DysonSphereBlueprints.ItemStore.Enums;
using DysonSphereBlueprints.Web.Code.Model;
using Radzen;

namespace DysonSphereBlueprints.Web.Code.Modifiers;

public class TweakSetProliferators(NotificationService notificationService)
    : BlueprintAction<BlueprintLogisticsStationModel>(notificationService)
{
    private record struct DesiredConfig(int Max, LogisticRole LocalLogic, LogisticRole RemoteLogic);

    public override string Title => "Set proliferator demand";
    public override string Description => "Set proliferators to local demand 1.000, unless exporting";

    protected override IEnumerable<BlueprintLogisticsStationModel> DiscoverEntries(BlueprintEditModel bpModel)
    {
        return bpModel.PlanetaryLogisticsStations
            .Cast<BlueprintLogisticsStationModel>()
            .Concat(bpModel.InterstellarLogisticsStations);
    }

    private DesiredConfig CalculateDesired(BlueprintLogisticsStationModel entry,
        BlueprintLogisticsStationStorageModel storage)
    {
        // Determine if this is exporting
        bool isExporting =
            entry.Slots.Any(x => x.StorageSlot == storage.Index && x.Direction == StationDirection.Output);
        if (isExporting)
            return new DesiredConfig(5000, LogisticRole.Demand, LogisticRole.None);
        return new DesiredConfig(1000, LogisticRole.Demand, LogisticRole.None);
    }

    public override bool CanApply(BlueprintLogisticsStationModel entry)
    {
        IEnumerable<BlueprintLogisticsStationStorageModel> storages =
            entry.Storages.Where(s =>
                s.Item is DspItem.ProliferatorMkI or DspItem.ProliferatorMkII or DspItem.ProliferatorMkIII);

        foreach (BlueprintLogisticsStationStorageModel storage in storages)
        {
            DesiredConfig desired = CalculateDesired(entry, storage);
            if (storage.Max != desired.Max || storage.LocalLogic != desired.LocalLogic ||
                storage.RemoteLogic != desired.RemoteLogic)
                return true;
        }

        return false;
    }

    protected override void PerformSingle(BlueprintLogisticsStationModel entry)
    {
        IEnumerable<BlueprintLogisticsStationStorageModel> storages =
            entry.Storages.Where(s =>
                s.Item is DspItem.ProliferatorMkI or DspItem.ProliferatorMkII or DspItem.ProliferatorMkIII);

        foreach (BlueprintLogisticsStationStorageModel storage in storages)
        {
            DesiredConfig desired = CalculateDesired(entry, storage);
            storage.Max = desired.Max;
            storage.LocalLogic = desired.LocalLogic;
            storage.RemoteLogic = desired.RemoteLogic;
        }
    }
}