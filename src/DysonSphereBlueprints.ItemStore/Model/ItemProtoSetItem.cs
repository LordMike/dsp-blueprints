namespace DysonSphereBlueprints.ItemStore.Model;

public class ItemProtoSetItem
{
    public string Name { get; init; }
    public int ID { get; init; }
    public string SID { get; init; }
    public string Type { get; init; }
    public int SubID { get; init; }
    public string MiningFrom { get; init; }
    public string ProduceFrom { get; init; }
    public int StackSize { get; init; }
    public int Grade { get; init; }
    public int[] Upgrades { get; init; }
    public bool IsFluid { get; init; }
    public bool IsEntity { get; init; }
    public bool CanBuild { get; init; }
    public bool BuildInGas { get; init; }
    public string IconPath { get; init; }
    public int ModelIndex { get; init; }
    public int ModelCount { get; init; }
    public int HpMax { get; init; }
    public int Ability { get; init; }
    public long HeatValue { get; init; }
    public int Potential { get; init; }
    public double ReactorInc { get; init; }
    public int FuelType { get; init; }
    public string AmmoType { get; init; }
    public string BombType { get; init; }
    public int CraftType { get; init; }
    public int BuildIndex { get; init; }
    public int BuildMode { get; init; }
    public int GridIndex { get; init; }
    public int UnlockKey { get; init; }
    public int PreTechOverride { get; init; }
    public bool Productive { get; init; }
    public int MechaMaterialID { get; init; }
    public double DropRate { get; init; }
    public int EnemyDropLevel { get; init; }
    public Position EnemyDropRange { get; init; }
    public double EnemyDropCount { get; init; }
    public long EnemyDropMask { get; init; }
    public double EnemyDropMaskRatio { get; init; }
    public int[] DescFields { get; init; }
    public string Description { get; init; }
}