using System.ComponentModel;
using System.Runtime.CompilerServices;
using DysonSphereBlueprints.ItemStore.Enums;

namespace DysonSphereBlueprints.Web.Code.Model;

public sealed class BlueprintEditModel : INotifyPropertyChanged
{
    public BlueprintEditModel(BlueprintData blueprint)
    {
        Blueprint = blueprint;

        for (int index = 0; index < blueprint.buildings.Length; index++)
        {
            BlueprintBuilding? building = blueprint.buildings[index];
            DspItem itemId = (DspItem)building.itemId;
            if (itemId is DspItem.PlanetaryLogisticsStation)
            {
                BlueprintPlanetaryLogisticsStationModel model =
                    new BlueprintPlanetaryLogisticsStationModel(building, index, itemId);
                PlanetaryLogisticsStations.Add(model);
                model.PropertyChanged += (_, _) =>
                {
                    NotifyPropertyChanged(nameof(PlanetaryLogisticsStations));
                    SetModified(true);
                };
            }

            if (itemId is DspItem.InterstellarLogisticsStation)
            {
                BlueprintInterstellarLogisticsStationModel model =
                    new BlueprintInterstellarLogisticsStationModel(building, index, itemId);
                InterstellarLogisticsStations.Add(model);
                model.PropertyChanged += (_, _) =>
                {
                    NotifyPropertyChanged(nameof(InterstellarLogisticsStations));
                    SetModified(true);
                };
            }
        }
    }

    public bool Modified { get; private set; }
    internal BlueprintData Blueprint { get; }

    public string Title
    {
        get => Blueprint.shortDesc;
        set
        {
            if (Blueprint.shortDesc == value)
                return;

            Blueprint.shortDesc = value;
            NotifyPropertyChanged();
            SetModified(true);
        }
    }

    public string Description
    {
        get => Blueprint.desc;
        set
        {
            if (Blueprint.desc == value)
                return;

            Blueprint.desc = value;
            NotifyPropertyChanged();
            SetModified(true);
        }
    }

    public string GameVersion
    {
        get => Blueprint.gameVersion;
        set
        {
            if (Blueprint.gameVersion == value)
                return;

            Blueprint.gameVersion = value;
            NotifyPropertyChanged();
            SetModified(true);
        }
    }

    public List<BlueprintPlanetaryLogisticsStationModel> PlanetaryLogisticsStations { get; set; } = new();
    public List<BlueprintInterstellarLogisticsStationModel> InterstellarLogisticsStations { get; set; } = new();

    public void SetModified(bool modified)
    {
        Modified = modified;
        NotifyPropertyChanged(nameof(Modified));
    }

    public string RenderBlueprint() => Blueprint.ToBase64String();

    public event PropertyChangedEventHandler? PropertyChanged;

    private void NotifyPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}