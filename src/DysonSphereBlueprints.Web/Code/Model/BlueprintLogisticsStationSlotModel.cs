using System.ComponentModel;
using System.Runtime.CompilerServices;
using DysonSphereBlueprints.Analysis.Analysis;

namespace DysonSphereBlueprints.Web.Code.Model;

public sealed record BlueprintLogisticsStationSlotModel(BlueprintLogisticsStationModel LogisticsStationModel, int Index)
    : INotifyPropertyChanged
{
    public StationDirection Direction
    {
        get => (StationDirection)LogisticsStationModel.Reference.parameters[192 + Index * 4];
        set
        {
            LogisticsStationModel.Reference.parameters[192 + Index * 4] = (int)value;
            NotifyPropertyChanged();
        }
    }

    public int? StorageSlot
    {
        get
        {
            var val = LogisticsStationModel.Reference.parameters[192 + Index * 4 + 1];
            if (val == 0)
                return null;
            return val - 1;
        }
        set
        {
            LogisticsStationModel.Reference.parameters[192 + Index * 4 + 1] = value switch
            {
                null => 0,
                _ => value.Value + 1
            };
            NotifyPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void NotifyPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}