using DysonSphereBlueprints.Analysis.Enums;

namespace DysonSphereBlueprints.Web.Code.Model;

public class BlueprintInterstellarLogisticsStationModel(
    BlueprintEditModel editModel,
    BlueprintBuilding reference,
    int id,
    DspItem building)
    : BlueprintLogisticsStationModel(editModel, reference, id, building)
{
    public bool FillVessels
    {
        get => Reference.parameters[331] == 1;
        set => SetParameter(331, value ? 1 : 0);
    }

    public bool UseOrbitalCollectors
    {
        get => Reference.parameters[323] == 1;
        set => SetParameter(323, value ? 1 : 0);
    }

    public int WarperDistance
    {
        get => Reference.parameters[324];
        set => SetParameter(324, value);
    }

    public bool RequireWarpers
    {
        get => Reference.parameters[325] == 1;
        set => SetParameter(325, value ? 1 : 0);
    }

    /// <summary>
    /// In percent, 0..100; use 10-step
    /// </summary>
    public int MinVesselLoad
    {
        get => Reference.parameters[327];
        set => SetParameter(327, value);
    }

    public bool HasWarperSlot => StorageSlots.Any(x => x.Item == DspItem.SpaceWarper);
}