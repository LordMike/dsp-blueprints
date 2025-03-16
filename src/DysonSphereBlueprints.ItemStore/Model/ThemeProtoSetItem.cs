namespace DysonSphereBlueprints.ItemStore.Model;

public class ThemeProtoSetItem
{
    public string Name { get; init; }
    public int ID { get; init; }
    public string SID { get; init; }
    public string DisplayName { get; init; }
    public string BriefIntroduction { get; init; }
    public string PlanetType { get; init; }
    public string MaterialPath { get; init; }
    public double Temperature { get; init; }
    public string Distribute { get; init; }
    public int[] Algos { get; init; }
    public Position ModX { get; init; }
    public Position ModY { get; init; }
    public int EigenBit { get; init; }
    public int[] Vegetables0 { get; init; }
    public int[] Vegetables1 { get; init; }
    public int[] Vegetables2 { get; init; }
    public int[] Vegetables3 { get; init; }
    public int[] Vegetables4 { get; init; }
    public int[] Vegetables5 { get; init; }
    public int[] VeinSpot { get; init; }
    public double[] VeinCount { get; init; }
    public double[] VeinOpacity { get; init; }
    public int[] RareVeins { get; init; }
    public double[] RareSettings { get; init; }
    public int[] GasItems { get; init; }
    public double[] GasSpeeds { get; init; }
    public bool UseHeightForBuild { get; init; }
    public double Wind { get; init; }
    public int IonHeight { get; init; }
    public double WaterHeight { get; init; }
    public int WaterItemId { get; init; }
    public int[] Musics { get; init; }
    public string SFXPath { get; init; }
    public double SFXVolume { get; init; }
    public int CullingRadius { get; init; }
    public int IceFlag { get; init; }
}