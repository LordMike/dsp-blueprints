using DysonSphereBlueprints.Analysis.Enums;

namespace DysonSphereBlueprints.Web.Code.Model;

public class BlueprintPlanetaryLogisticsStationModel(
    BlueprintEditModel editModel,
    BlueprintBuilding reference,
    int id,
    DspItem building)
    : BlueprintLogisticsStationModel(editModel, reference, id, building);