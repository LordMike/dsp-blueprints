using System.ComponentModel;
using System.Runtime.CompilerServices;
using DysonSphereBlueprints.Analysis.Analysis;
using DysonSphereBlueprints.ItemStore.Enums;

namespace DysonSphereBlueprints.Web.Code.Model;

public abstract class BlueprintLogisticsStationModel : INotifyPropertyChanged
{
    protected BlueprintLogisticsStationModel(BlueprintBuilding reference,
        int id,
        DspItem building)
    {
        Reference = reference;
        Id = id;
        Building = building;

        StationInfo info = StationInfo.FromParameters(building, reference.parameters);
        BlueprintLogisticsStationStorageModel[] storageViews = info.Storage
            .Select((_, idx) =>
            {
                BlueprintLogisticsStationStorageModel model = new BlueprintLogisticsStationStorageModel(this, idx);
                model.PropertyChanged += (_, _) => NotifyPropertyChanged(nameof(StorageSlots));
                return model;
            })
            .ToArray();

        StorageSlots = storageViews;
    }

    internal BlueprintBuilding Reference { get; init; }
    public int Id { get; init; }
    public DspItem Building { get; init; }

    public BlueprintLogisticsStationStorageModel[] StorageSlots { get; init; }

    public bool FillDrones
    {
        get => Reference.parameters[330] == 1;
        set
        {
            Reference.parameters[330] = value ? 1 : 0;
            NotifyPropertyChanged();
        }
    }

    /// <summary>
    /// In percent, 0..100; use 10-step
    /// </summary>
    public int MinDroneLoad
    {
        get => Reference.parameters[326];
        set
        {
            Reference.parameters[326] = value;
            NotifyPropertyChanged();
        }
    }

    /// <summary>
    /// 1..4; 0: always use tech limit
    /// </summary>
    public int StackCount
    {
        get => Reference.parameters[328];
        set
        {
            Reference.parameters[328] = value;
            NotifyPropertyChanged();
        }
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void NotifyPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}