﻿using DysonSphereBlueprints.Gamelibs.Code;
using DysonSphereBlueprints.ItemStore.Enums;

namespace DysonSphereBlueprints.Web.Code.Model;

public sealed class BlueprintPlanetaryLogisticsStationModel(
    BlueprintBuilding reference,
    int id,
    DspItem building)
    : BlueprintLogisticsStationModel(reference, id, building);