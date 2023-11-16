using System.Numerics;
using BiSharper.Rv.Shape.Flags;
using BiSharper.Rv.Shape.Flags.Internal;

namespace BiSharper.Rv.Shape.Types;

public readonly struct ShapePoint
{
    public DetailLevel LOD { get; private init; }
    public float X { get; private init; }
    public float Y { get; private init; }
    public float Z { get; private init; }
    public bool Hidden { get; private init; }
    public ShapeHint Hints { get; private init; }
    
    public static ShapePoint[] ReadMulti(BinaryReader reader, bool extended, DetailLevel parent, int count)
    {
        var points = new ShapePoint[count];
        for (var i = 0; i < count; i++) points[i] = new ShapePoint(reader, extended, parent);
        return points;
    }
    
    public static implicit operator Vector3(ShapePoint point) => new(point.X, point.Y, point.Z);

    public ShapePoint(BinaryReader reader, bool extended, DetailLevel parent)
    {
        LOD = parent;
        X = reader.ReadSingle();
        Y = reader.ReadSingle();
        Z = reader.ReadSingle();
        Hints = ShapeHint.ClipAll;
        Hidden = false;
        if (extended)
        {

            var remarks = reader.ReadInt32();
            if ((remarks & ~PointConstants.All) != 0)
            {
                //Warn invalid flags
            }
            else
            {
                if ((remarks & PointConstants.OnLand) != 0) Hints |= ShapeHint.LandOn;
                else if((remarks & PointConstants.UnderLand) != 0) Hints |= ShapeHint.LandUnder;
                else if((remarks & PointConstants.AboveLand) != 0) Hints |= ShapeHint.LandAbove;
                else if((remarks & PointConstants.KeepLand) != 0) Hints |= ShapeHint.LandKeep;

                if ((remarks & PointConstants.Decal) != 0) Hints |= ShapeHint.DecalNormal;
                else if ((remarks & PointConstants.VerticalDecal) != 0) Hints |= ShapeHint.DecalVertical;

                if ((remarks & PointConstants.NoLight) != 0)  Hints |= ShapeHint.ShineLightHints;
                else if ((remarks & PointConstants.FullLight) != 0)  Hints |= ShapeHint.FullLightHints;
                else if ((remarks & PointConstants.HalfLight) != 0)  Hints |= ShapeHint.HalfLightHints;
                else if ((remarks & PointConstants.AmbientLight) != 0)  Hints |= ShapeHint.AmbientLightHints;

                if ((remarks & PointConstants.NoFog) != 0)  Hints |= ShapeHint.FogDisable;
                else if ((remarks & PointConstants.SkyFog) != 0)  Hints |= ShapeHint.FogSky;

                if ((remarks & PointConstants.UserMask) != 0)
                {
                    var user = (remarks & PointConstants.UserMask) / PointConstants.UserStep;
                    var userHint = user * PointConstants.UserStep;
                    Hints |= (ShapeHint)userHint;
                }

                if ((remarks & PointConstants.HiddenSpecial) != 0) Hidden = true;
            }
        }
    }

}