using DysonSphereBlueprints.Analysis.Enums;

namespace DysonSphereBlueprints.Analysis.Analysis;

public class StationInfo
{
    public int workEnergyPerTick;
    public int tripRangeOfDrones;
    public int tripRangeOfShips;
    public bool includeOrbitCollector;
    public int warpEnableDistance;
    public bool warperNecessary;
    public int deliveryAmountOfDrones;
    public int deliveryAmountOfShips;
    public int pilerCount;
    public bool droneAutoReplenish;
    public bool shipAutoReplenish;
    public StationStorageItem[] Storage;
    public StationSlotItem[] Slots;

    public static StationInfo FromParameters(DspItem itemId, int[] parameters)
    {
        // https://github.com/huww98/dsp_blueprint_editor/blob/5b1a2588243a9bf7115c00ede4757aa42e719dbe/src/blueprint/parser.ts#L174
        (int maxItemKind, int numSlots) spec = itemId switch
        {
            DspItem.PlanetaryLogisticsStation => (4, 12),
            DspItem.InterstellarLogisticsStation => (5, 12),
            DspItem.AdvancedMiningMachine => (1, 9),
            _ => throw new ArgumentOutOfRangeException(nameof(itemId), itemId, null)
        };

        return new StationInfo
        {
            workEnergyPerTick = parameters[320],
            tripRangeOfDrones = parameters[321], // * 100000000.0,
            tripRangeOfShips = parameters[322] / 100,
            includeOrbitCollector = parameters[323] == 1,
            warpEnableDistance = parameters[324],
            warperNecessary = parameters[325] == 1,
            deliveryAmountOfDrones = parameters[326],
            deliveryAmountOfShips = parameters[327],
            pilerCount = parameters[328],
            droneAutoReplenish = parameters[330] == 1,
            shipAutoReplenish = parameters[331] == 1,
            Storage = parameters
                .Take(spec.maxItemKind * 6)
                .Chunk(6)
                .Select(s => new StationStorageItem(s[0], (LogisticRole)s[1], (LogisticRole)s[2], s[3], s[4]))
                .ToArray(),
            Slots = parameters
                .Skip(192)
                .Take(spec.numSlots * 4)
                .Chunk(4)
                .Select(s => new StationSlotItem((StationDirection)s[0], s[1]))
                .ToArray(),
        };
    }
}