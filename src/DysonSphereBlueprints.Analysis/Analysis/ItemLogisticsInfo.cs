namespace DysonSphereBlueprints.Analysis.Analysis;

public record struct ItemLogisticsInfo(
    int ItemId,
    bool LocalSupply,
    bool LocalDemand,
    bool RemoteSupply,
    bool RemoteDemand,
    int TotalMaxStorage);