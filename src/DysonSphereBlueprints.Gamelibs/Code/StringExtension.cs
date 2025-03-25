using System;

namespace DysonSphereBlueprints.Gamelibs.Code;

public static class StringExtension
{
    public static string Escape(this string s) => Uri.EscapeDataString(s);

    public static string Unescape(this string s) => Uri.UnescapeDataString(s);
}