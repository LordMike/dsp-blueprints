namespace DysonSphereBlueprints.Gamelibs.Code;

public static class StringConversion
{
    public static bool ToInt(this string s, out int o) => int.TryParse(s, out o);

    public static bool ToLong(this string s, out long o) => long.TryParse(s, out o);
}