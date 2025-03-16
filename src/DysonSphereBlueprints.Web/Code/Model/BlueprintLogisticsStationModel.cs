using DysonSphereBlueprints.Analysis.Analysis;
using DysonSphereBlueprints.Analysis.Enums;

namespace DysonSphereBlueprints.Web.Code.Model;

public abstract class BlueprintLogisticsStationModel
{
    public BlueprintLogisticsStationModel(BlueprintEditModel editModel,
        BlueprintBuilding reference,
        int id,
        DspItem building)
    {
        EditModel = editModel;
        Reference = reference;
        Id = id;
        Building = building;

        StationInfo info = StationInfo.FromParameters(building, reference.parameters);
        BlueprintLogisticsStationStorageModel[] storageViews = info.Storage
            .Select((_, idx) =>
                new BlueprintLogisticsStationStorageModel(this, idx))
            .ToArray();

        StorageSlots = storageViews;
    }

    internal BlueprintEditModel EditModel { get; init; }
    internal BlueprintBuilding Reference { get; init; }
    public int Id { get; init; }
    public DspItem Building { get; init; }

    public BlueprintLogisticsStationStorageModel[] StorageSlots { get; init; }

    public bool FillDrones
    {
        get => Reference.parameters[330] == 1;
        set => SetParameter(330, value ? 1 : 0);
    }

    /// <summary>
    /// In percent, 0..100; use 10-step
    /// </summary>
    public int MinDroneLoad
    {
        get => Reference.parameters[326];
        set => SetParameter(326, value);
    }

    /// <summary>
    /// 1..4; 0: always use tech limit
    /// </summary>
    public int StackCount
    {
        get => Reference.parameters[328];
        set => SetParameter(328, value);
    }

    public void SetParameter(int index, int newValue)
    {
        var prevValue = Reference.parameters[index];
        if (prevValue != newValue)
        {
            Reference.parameters[index] = newValue;
            EditModel.SetModified(true);
        }
    }
}