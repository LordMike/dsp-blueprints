namespace DysonSphereBlueprints.ItemStore.Model;

public class VeinProtoSetItem
{
    public string Name { get; init; }
    public int ID { get; init; }
    public string SID { get; init; }
    public string IconPath { get; init; }
    public string Description { get; init; }
    public int ModelIndex { get; init; }
    public int ModelCount { get; init; }
    public double CircleRadius { get; init; }
    public int MiningItem { get; init; }
    public int MiningTime { get; init; }
    public int MiningAudio { get; init; }
    public int MiningEffect { get; init; }
    public int MinerBaseModelIndex { get; init; }
    public int MinerCircleModelIndex { get; init; }
    public object prefabDesc { get; init; }
}