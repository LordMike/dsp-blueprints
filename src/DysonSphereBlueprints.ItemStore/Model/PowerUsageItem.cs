namespace DysonSphereBlueprints.ItemStore.Model;

public class PowerUsageItem
{
    public int Id { get; set; }
    public PowerUsageRange Uses { get; init; }
    public PowerUsageRange Provides { get; init; }
}