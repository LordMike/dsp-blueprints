namespace DysonSphereBlueprints.ItemStore.Model;

public class AchievementProtoSetItem
{
    public string Name { get; init; }
    public int ID { get; init; }
    public string SID { get; init; }
    public string Description { get; init; }
    public long TargetValue { get; init; }
    public int ProgressChangeStep { get; init; }
    public string IconPath { get; init; }
    public string PlatformSID { get; init; }
    public string XGPSID { get; init; }
    public string SteamStatSID { get; init; }
    public string[] SyncPlatformProgress { get; init; }
    public bool ClearWhenLoad { get; init; }
    public bool AllowDecrease { get; init; }
    public bool AllowUseProperty { get; init; }
    public string DeterminatorName { get; init; }
    public long[] DeterminatorParams { get; init; }
    public string RuntimeDataName { get; init; }
}