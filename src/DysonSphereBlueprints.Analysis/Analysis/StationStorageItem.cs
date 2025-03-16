namespace DysonSphereBlueprints.Analysis.Analysis;

public record StationStorageItem(int ItemId, LogisticRole LocalLogic, LogisticRole RemoteLogic, int Max, int KeepMode);