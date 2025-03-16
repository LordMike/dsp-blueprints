using DysonSphereBlueprints.Analysis.Enums;

namespace DysonSphereBlueprints.Web.Code.Model;

public class BlueprintEditModel
{
    public BlueprintEditModel(BlueprintData blueprint)
    {
        Blueprint = blueprint;

        for (int index = 0; index < blueprint.buildings.Length; index++)
        {
            BlueprintBuilding? building = blueprint.buildings[index];
            DspItem itemId = (DspItem)building.itemId;
            if (itemId is DspItem.PlanetaryLogisticsStation)
                PlanetaryLogisticsStations.Add(
                    new BlueprintPlanetaryLogisticsStationModel(this, building, index, itemId));

            if (itemId is DspItem.InterstellarLogisticsStation)
                InterstellarLogisticsStations.Add(
                    new BlueprintInterstellarLogisticsStationModel(this, building, index, itemId));
        }
    }

    public bool Modified { get; private set; }
    internal BlueprintData Blueprint { get; }

    public string Title
    {
        get => Blueprint.desc;
        set
        {
            if (Blueprint.desc == value)
                return;
            
            Blueprint.desc = value;
            SetModified(true);
        }
    }

    public string Description
    {
        get => Blueprint.shortDesc;
        set
        {
            if (Blueprint.shortDesc == value)
                return;

            Blueprint.shortDesc = value;
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
            SetModified(true);
        }
    }

    public List<BlueprintPlanetaryLogisticsStationModel> PlanetaryLogisticsStations { get; set; } = new();
    public List<BlueprintInterstellarLogisticsStationModel> InterstellarLogisticsStations { get; set; } = new();

    public void SetModified(bool modified) => Modified = modified;

    public string RenderBlueprint() => Blueprint.ToBase64String();
}