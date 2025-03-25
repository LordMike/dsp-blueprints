using System.IO;

namespace DysonSphereBlueprints.Gamelibs.Code;

public class BlueprintArea
{
    public int index;
    public int parentIndex;
    public int tropicAnchor;
    public int areaSegments;
    public int anchorLocalOffsetX;
    public int anchorLocalOffsetY;
    public int width;
    public int height;

    public void Import(BinaryReader r)
    {
        index = r.ReadSByte();
        parentIndex = r.ReadSByte();
        tropicAnchor = r.ReadInt16();
        areaSegments = r.ReadInt16();
        anchorLocalOffsetX = r.ReadInt16();
        anchorLocalOffsetY = r.ReadInt16();
        width = r.ReadInt16();
        height = r.ReadInt16();
    }

    public void Export(BinaryWriter w)
    {
        w.Write((sbyte)index);
        w.Write((sbyte)parentIndex);
        w.Write((short)tropicAnchor);
        w.Write((short)areaSegments);
        w.Write((short)anchorLocalOffsetX);
        w.Write((short)anchorLocalOffsetY);
        w.Write((short)width);
        w.Write((short)height);
    }
}