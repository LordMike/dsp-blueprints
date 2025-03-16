using DysonSphereBlueprints.Analysis.Analysis;
using DysonSphereBlueprints.Web.Code.Model;
using Radzen;

namespace DysonSphereBlueprints.Web.Code.Modifiers;

public class TweakSetFills(BlueprintEditModel bpModel, NotificationService notificationService)
    : BlueprintAction<BlueprintLogisticsStationModel>(notificationService)
{
    public override string Title => "Set fills";
    public override string Description => "Set drone/vessel fills as required";

    protected override IEnumerable<BlueprintLogisticsStationModel> DiscoverEntries()
    {
        return bpModel.PlanetaryLogisticsStations
            .Cast<BlueprintLogisticsStationModel>()
            .Concat(bpModel.InterstellarLogisticsStations);
    }

    public override bool CanApply(BlueprintLogisticsStationModel entry)
    {
        return entry.StorageSlots.Any(s => s.LocalLogic != LogisticRole.None && s.Max != 0) ||
               (entry is BlueprintInterstellarLogisticsStationModel inter &&
                inter.StorageSlots.Any(s => s.RemoteLogic != LogisticRole.None && s.Max != 0));
    }

    protected override BlueprintActionResult PerformSingle(BlueprintLogisticsStationModel entry)
    {
        bool shouldHaveDrones = entry.StorageSlots.Any(s => s.LocalLogic != LogisticRole.None && s.Max != 0);
        bool shouldHaveVessels = entry.StorageSlots.Any(s => s.RemoteLogic != LogisticRole.None && s.Max != 0);

        if (entry is BlueprintPlanetaryLogisticsStationModel planetary)
        {
            if (planetary.FillDrones != shouldHaveDrones)
            {
                planetary.FillDrones = shouldHaveDrones;
                return BlueprintActionResult.Success;
            }
        }

        if (entry is BlueprintInterstellarLogisticsStationModel interstellar)
        {
            if (interstellar.FillDrones != shouldHaveDrones ||
                interstellar.FillVessels != shouldHaveVessels)
            {
                interstellar.FillDrones = shouldHaveDrones;
                interstellar.FillVessels = shouldHaveVessels;
                return BlueprintActionResult.Success;
            }
        }

        return BlueprintActionResult.Skipped; // Null means already set / skipped
    }
}