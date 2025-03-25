using DysonSphereBlueprints.Gamelibs.Code;
using DysonSphereBlueprints.ItemStore.Enums;

namespace DysonSphereBlueprints.Web.Code.Model;

public sealed class BlueprintInterstellarLogisticsStationModel(
    BlueprintBuilding reference,
    int id,
    DspItem building)
    : BlueprintLogisticsStationModel(reference, id, building)
{
    public bool FillVessels
    {
        get => Reference.parameters[331] == 1;
        set
        {
            Reference.parameters[331] = value ? 1 : 0;
            NotifyPropertyChanged();
        }
    }

    public bool UseOrbitalCollectors
    {
        get => Reference.parameters[323] == 1;
        set
        {
            Reference.parameters[323] = value ? 1 : 0;
            NotifyPropertyChanged();
        }
    }

    public int WarperDistance
    {
        get => Reference.parameters[324];
        set
        {
            Reference.parameters[324] = value;
            NotifyPropertyChanged();
        }
    }

    public bool RequireWarpers
    {
        get => Reference.parameters[325] == 1;
        set
        {
            Reference.parameters[325] = value ? 1 : 0;
            NotifyPropertyChanged();
        }
    }

    /// <summary>
    /// In percent, 0..100; use 10-step
    /// </summary>
    public int MinVesselLoad
    {
        get => Reference.parameters[327];
        set
        {
            Reference.parameters[327] = value;
            NotifyPropertyChanged();
        }
    }

    public bool HasWarperSlot => Storages.Any(x => x.Item == DspItem.SpaceWarper);
}