using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using DysonSphereBlueprints.Analysis.Analysis;
using DysonSphereBlueprints.ItemStore.Enums;

namespace DysonSphereBlueprints.Web.Code.Model;

public sealed record BlueprintLogisticsStationStorageModel(BlueprintLogisticsStationModel LogisticsStationModel, int Index)
    : INotifyPropertyChanged
{
    public DspItem Item
    {
        get => (DspItem)LogisticsStationModel.Reference.parameters[Index * 6];
        set
        {
            LogisticsStationModel.Reference.parameters[Index * 6] = (int)value;
            NotifyPropertyChanged();
        }
    }

    public LogisticRole LocalLogic
    {
        get => (LogisticRole)LogisticsStationModel.Reference.parameters[Index * 6 + 1];
        set
        {
            if (BitOperations.PopCount((uint)value) > 1)
                throw new ArgumentException("LogisticsRole must be one value only", nameof(value));
            
            LogisticsStationModel.Reference.parameters[Index * 6 + 1] = (int)value;
            NotifyPropertyChanged();
        }
    }


    public LogisticRole RemoteLogic
    {
        get => (LogisticRole)LogisticsStationModel.Reference.parameters[Index * 6 + 2];
        set
        {
            if (BitOperations.PopCount((uint)value) > 1)
                throw new ArgumentException("LogisticsRole must be one value only", nameof(value));

            LogisticsStationModel.Reference.parameters[Index * 6 + 2] = (int)value;
            NotifyPropertyChanged();
        }
    }

    /// <summary>
    /// 0..20000
    /// </summary>
    public int Max
    {
        get => LogisticsStationModel.Reference.parameters[Index * 6 + 3];
        set
        {
            LogisticsStationModel.Reference.parameters[Index * 6 + 3] = value;
            NotifyPropertyChanged();
        }
    }

    public int KeepMode
    {
        get => LogisticsStationModel.Reference.parameters[Index * 6 + 4];
        set
        {
            LogisticsStationModel.Reference.parameters[Index * 6 + 4] = value;
            NotifyPropertyChanged();
        }
    }

    public bool HasItem => Item != 0;
    
    public event PropertyChangedEventHandler? PropertyChanged;

    private void NotifyPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}