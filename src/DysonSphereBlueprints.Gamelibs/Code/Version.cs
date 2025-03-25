using System;

namespace DysonSphereBlueprints.Gamelibs.Code;

public struct Version(int major, int minor, int release)
{
    public int Major = major;
    public int Minor = minor;
    public int Release = release;
    public int Build = 0;

    public int sig => Major << 24 | Minor << 16 | Release;

    public static bool operator ==(Version lhs, Version rhs)
    {
        return lhs.Major == rhs.Major && lhs.Minor == rhs.Minor && lhs.Release == rhs.Release;
    }

    public static bool operator !=(Version lhs, Version rhs)
    {
        return lhs.Major != rhs.Major || lhs.Minor != rhs.Minor || lhs.Release != rhs.Release;
    }

    public static bool operator >(Version lhs, Version rhs)
    {
        if (lhs.sig > rhs.sig)
            return true;
        return lhs.sig >= rhs.sig && lhs.Build > rhs.Build;
    }

    public static bool operator <(Version lhs, Version rhs)
    {
        if (lhs.sig < rhs.sig)
            return true;
        return lhs.sig <= rhs.sig && lhs.Build < rhs.Build;
    }

    public override bool Equals(object obj)
    {
        return obj != null && obj is Version version && this == version;
    }

    public override int GetHashCode() => sig;

    public override string ToString()
    {
        return string.Format("{0}.{1}.{2}", Major, Minor, Release);
    }

    public string ToFullString()
    {
        return string.Format("{0}.{1}.{2}.{3}", Major, Minor, Release, Build);
    }

    public void FromFullString(string s)
    {
        string[] strArray = s.Split(new char[1]{ '.' }, StringSplitOptions.RemoveEmptyEntries);
        if (strArray == null)
            return;
        if (strArray.Length != 0)
            int.TryParse(strArray[0], out Major);
        if (strArray.Length > 1)
            int.TryParse(strArray[1], out Minor);
        if (strArray.Length > 2)
            int.TryParse(strArray[2], out Release);
        if (strArray.Length <= 3)
            return;
        int.TryParse(strArray[3], out Build);
    }
}