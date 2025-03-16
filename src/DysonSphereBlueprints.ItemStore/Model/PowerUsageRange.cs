namespace DysonSphereBlueprints.ItemStore.Model;

public record struct PowerUsageRange
{
    public int Low { get; init; }
    public int High { get; init; }

    public static PowerUsageRange operator +(PowerUsageRange lhs, PowerUsageRange rhs)
    {
        return new PowerUsageRange
        {
            Low = lhs.Low + rhs.Low,
            High = lhs.High + rhs.High
        };
    }

    public static PowerUsageRange operator *(PowerUsageRange lhs, int count)
    {
        return new PowerUsageRange
        {
            Low = lhs.Low * count,
            High = lhs.High * count
        };
    }
}