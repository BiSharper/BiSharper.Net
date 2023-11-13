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
    
    //maybe private level for merge into hints
    //public PointRemark Remarks { get; private init; }

    public ShapePoint(BinaryReader reader, bool extended, DetailLevel parent)
    {
        LOD = parent;
        X = reader.ReadSingle();
        Y = reader.ReadSingle();
        Z = reader.ReadSingle();
        Hints = PointHint.ClipAll;
        Hidden = false;
        if (extended)
        {
            var remarks = (PointRemark)reader.ReadInt32();
            if ((remarks & ~PointRemark.All) != 0)
            {
                //Warn invalid flags
            }
            else
            {
                if ((remarks & PointRemark.OnLand) != 0) Hints |= PointHint.LandOn;
                else if((remarks & PointRemark.UnderLand) != 0) Hints |= PointHint.LandUnder;
                else if((remarks & PointRemark.AboveLand) != 0) Hints |= PointHint.LandAbove;
                else if((remarks & PointRemark.KeepLand) != 0) Hints |= PointHint.LandKeep;

                if ((remarks & PointRemark.Decal) != 0) Hints |= PointHint.DecalNormal;
                else if ((remarks & PointRemark.VerticalDecal) != 0) Hints |= PointHint.DecalVertical;

                if ((remarks & PointRemark.NoLight) != 0)  Hints |= PointHint.ShineLightHints;
                else if ((remarks & PointRemark.FullLight) != 0)  Hints |= PointHint.FullLightHints;
                else if ((remarks & PointRemark.HalfLight) != 0)  Hints |= PointHint.HalfLightHints;
                else if ((remarks & PointRemark.AmbientLight) != 0)  Hints |= PointHint.AmbientLightHints;

                if ((remarks & PointRemark.NoFog) != 0)  Hints |= PointHint.FogDisable;
                else if ((remarks & PointRemark.SkyFog) != 0)  Hints |= PointHint.FogSky;

                if ((remarks & PointRemark.UserMask) != 0)
                {
                    var user = (int) (remarks & PointRemark.UserMask) / (int)PointRemark.UserStep;
                    var userHint = user * (int)PointRemark.UserStep;
                    Hints |= (PointHint) userHint;
                }

                if ((remarks & PointRemark.HiddenSpecial) != 0) Hidden = true;
            }
        }
    }

}