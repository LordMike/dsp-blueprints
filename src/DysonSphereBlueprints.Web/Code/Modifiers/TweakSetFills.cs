using DysonSphereBlueprints.Analysis.Analysis;
using DysonSphereBlueprints.Web.Code.Model;
using Radzen;

namespace DysonSphereBlueprints.Web.Code.Modifiers;

public class TweakSetFills(NotificationService notificationService)
    : BlueprintAction<BlueprintLogisticsStationModel>(notificationService)
{
    public override string Title => "Set fills";
    public override string Description => "Set drone/vessel fills as required";

    protected override IEnumerable<BlueprintLogisticsStationModel> DiscoverEntries(BlueprintEditModel bpModel)
    {
        return bpModel.PlanetaryLogisticsStations
            .Cast<BlueprintLogisticsStationModel>()
            .Concat(bpModel.InterstellarLogisticsStations);
    }

    private (bool shouldHaveDrones, bool shouldHaveVessels) GetDesired(BlueprintLogisticsStationModel entry)
    {
        bool shouldHaveDrones = entry.StorageSlots.Any(s => s.LocalLogic != LogisticRole.None && s.Max != 0);
        bool shouldHaveVessels = entry.StorageSlots.Any(s => s.RemoteLogic != LogisticRole.None && s.Max != 0);

        return (shouldHaveDrones, shouldHaveVessels);
    }

    public override bool CanApply(BlueprintLogisticsStationModel entry)
    {
        (bool shouldHaveDrones, bool shouldHaveVessels) = GetDesired(entry);

        if (entry is BlueprintPlanetaryLogisticsStationModel planetary)
        {
            if (planetary.FillDrones != shouldHaveDrones)
                return true;
        }
        else if (entry is BlueprintInterstellarLogisticsStationModel interstellar)
        {
            if (interstellar.FillDrones != shouldHaveDrones ||
                interstellar.FillVessels != shouldHaveVessels)
                return true;
        }
        else
            throw new InvalidOperationException("Unexpected");

        return false;
    }

    protected override void PerformSingle(BlueprintLogisticsStationModel entry)
    {
        (bool shouldHaveDrones, bool shouldHaveVessels) = GetDesired(entry);

        if (entry is BlueprintPlanetaryLogisticsStationModel planetary)
            planetary.FillDrones = shouldHaveDrones;
        else if (entry is BlueprintInterstellarLogisticsStationModel interstellar)
        {
            interstellar.FillDrones = shouldHaveDrones;
            interstellar.FillVessels = shouldHaveVessels;
        }
    }
}