using DysonSphereBlueprints.Analysis.Analysis;
using DysonSphereBlueprints.Analysis.Enums;

namespace DysonSphereBlueprints.Web.Code.Model;

public record BlueprintLogisticsStationStorageModel(BlueprintLogisticsStationModel LogisticsStationModel, int Index)
{
    public DspItem Item
    {
        get => (DspItem)LogisticsStationModel.Reference.parameters[Index * 6];
        set => LogisticsStationModel.SetParameter(Index * 6, (int)value);
    }

    public LogisticRole LocalLogic
    {
        get => (LogisticRole)LogisticsStationModel.Reference.parameters[Index * 6 + 1];
        set => LogisticsStationModel.SetParameter(Index * 6 + 1, (int)value);
    }

    public LogisticRole RemoteLogic
    {
        get => (LogisticRole)LogisticsStationModel.Reference.parameters[Index * 6 + 2];
        set => LogisticsStationModel.SetParameter(Index * 6 + 2, (int)value);
    }

    /// <summary>
    /// 0..20000
    /// </summary>
    public int Max
    {
        get => LogisticsStationModel.Reference.parameters[Index * 6 + 3];
        set => LogisticsStationModel.SetParameter(Index * 6 + 3, value);
    }

    public int KeepMode
    {
        get => LogisticsStationModel.Reference.parameters[Index * 6 + 4];
        set => LogisticsStationModel.SetParameter(Index * 6 + 4, value);
    }

    public bool HasItem => Item != 0;

    // public bool IsUsedInSlot => LogisticsStationModel.Reference.Slots.Any(x => x.StorageIndex == idx)
}