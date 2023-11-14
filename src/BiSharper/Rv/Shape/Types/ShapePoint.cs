using BiSharper.Rv.Shape.Flags;

namespace BiSharper.Rv.Shape.Types;

public readonly struct ShapePoint
{
    public DetailLevel LOD { get; private init; }
    public float X { get; private init; }
    public float Y { get; private init; }
    public float Z { get; private init; }
    public bool Hidden { get; private init; }
    public PointHint Hints { get; private init; }
    
    public static ShapePoint[] ReadMulti(BinaryReader reader, bool extended, DetailLevel parent, int count)
    {
        var points = new ShapePoint[count];
        for (var i = 0; i < count; i++) points[i] = new ShapePoint(reader, extended, parent);
        return points;
    }

    public ShapePoint(BinaryReader reader, bool extended, DetailLevel parent)
    {
        const uint onLand = 0x1, underLand = 0x2, aboveLand = 0x4, keepLand = 0x8/*, landMask = 0xf*/;
        const uint decal = 0x100, verticalDecal = 0x200/*, decalMask = 0x300*/;
        const uint noLight = 0x10, ambientLight = 0x20, fullLight = 0x40, halfLight = 0x80/*, lightMask = 0xf0*/;
        const uint noFog = 0x1000, skyFog = 0x2000/*, fogMask = 0x3000*/;
        const uint userMask = 0xff0000, userStep = 0x010000;
        const uint specialMask = 0xf000000, hiddenSpecial = 0x1000000;
        const uint all = onLand | underLand | aboveLand | keepLand | decal | verticalDecal | noLight | fullLight |
                         halfLight | ambientLight | noFog | skyFog | userMask | specialMask;
        
        LOD = parent;
        X = reader.ReadSingle();
        Y = reader.ReadSingle();
        Z = reader.ReadSingle();
        Hints = PointHint.ClipAll;
        Hidden = false;
        if (extended)
        {

            var remarks = reader.ReadInt32();
            if ((remarks & ~all) != 0)
            {
                //Warn invalid flags
            }
            else
            {
                if ((remarks & onLand) != 0) Hints |= PointHint.LandOn;
                else if((remarks & underLand) != 0) Hints |= PointHint.LandUnder;
                else if((remarks & aboveLand) != 0) Hints |= PointHint.LandAbove;
                else if((remarks & keepLand) != 0) Hints |= PointHint.LandKeep;

                if ((remarks & decal) != 0) Hints |= PointHint.DecalNormal;
                else if ((remarks & verticalDecal) != 0) Hints |= PointHint.DecalVertical;

                if ((remarks & noLight) != 0)  Hints |= PointHint.ShineLightHints;
                else if ((remarks & fullLight) != 0)  Hints |= PointHint.FullLightHints;
                else if ((remarks & halfLight) != 0)  Hints |= PointHint.HalfLightHints;
                else if ((remarks & ambientLight) != 0)  Hints |= PointHint.AmbientLightHints;

                if ((remarks & noFog) != 0)  Hints |= PointHint.FogDisable;
                else if ((remarks & skyFog) != 0)  Hints |= PointHint.FogSky;

                if ((remarks & userMask) != 0)
                {
                    var user = (remarks & userMask) / userStep;
                    var userHint = user * userStep;
                    Hints |= (PointHint)userHint;
                }

                if ((remarks & hiddenSpecial) != 0) Hidden = true;
            }
        }
    }

}