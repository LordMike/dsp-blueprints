namespace DysonSphereBlueprints.ItemStore.Model;

public class AchievementProtoSet
{
    public string TableName { get; init; }
    public string Signature { get; init; }
    public AchievementProtoSetItem[] dataArray { get; init; }
}