using System.Numerics;
using System.Text;
using BiSharper.Common.IO;
using BiSharper.Rv.Shape.Flags;
using BiSharper.Rv.Shape.Flags.Internal;

namespace BiSharper.Rv.Shape.Types;

public readonly struct ShapeFace
{
    public DetailLevel LOD { get; private init; }
    public ShapeVertex[] Vertices { get; private init; }
    public string Texture { get; private init; }
    public string? Material { get; private init; }
    public ShapeHint Hints { get; private init; }
    public uint Spec { get; private init; }
    public int MinimumMaterial { get; private init; }
    public static readonly float MinimumNormal = (float)Math.Sqrt(0.1f);
    
    public static ShapeFace[] ReadMulti(BinaryReader reader, GeometryUsed geometryUsed, bool extended, bool material, DetailLevel parent, int count)
    {
        var faces = new ShapeFace[count];
        for (var i = 0; i < count; i++) faces[i] = new ShapeFace(reader, geometryUsed,  extended, material, parent);
        return faces;
    }

    public ShapeFace(BinaryReader reader, GeometryUsed geometryUsed, bool extended, bool material, DetailLevel parent)
    {
        LOD = parent;
        Hints = 0;
        uint hint;
        switch (material)
        {
            case true:
            {
                Vertices = ShapeVertex.ReadMulti(reader, geometryUsed, this, reader.ReadInt32());
                hint = (uint) reader.ReadInt32();
                Texture = reader.ReadAsciiZ().ToLower(); //RString::ReadString(QIStream &in)
                Material = reader.ReadAsciiZ().ToLower(); //RString::ReadString(QIStream &in)
                break;
            }
            case false:
            {
                Texture = Encoding.ASCII.GetString(reader.ReadBytes(32)).TrimEnd('\0').ToLower();
                Vertices = ShapeVertex.ReadMulti(reader, geometryUsed, this, reader.ReadInt32());
                Material = null;
                hint = extended ? (uint) reader.ReadInt32() : 0;
                break;
            }
        }

        switch (string.IsNullOrEmpty(Texture))
        {
            case true when string.IsNullOrEmpty(Material):
                //TODO: Assign default texture ln1113
                break;
            case false:
                //TODO: Resolve texture
                break;
        }

        if ((hint & FaceConstants.All) != 0)
        {
            if ((hint & FaceConstants.NoLight) != 0) Hints |= ShapeHint.ShineLightHints;
            else if ((hint & FaceConstants.AmbientLight) != 0) Hints |= ShapeHint.AmbientLightHints;
            else if ((hint & FaceConstants.FullLight) != 0) Hints |= ShapeHint.FullLightHints;
            
            if ((hint & FaceConstants.Shadow) != 0) Spec |= 0x40;
            if ((hint & FaceConstants.NoShadow) != 0) Spec |= 0x20;


            if ((hint & FaceConstants.UserMask) != 0)
            {
                var materialId = ((hint & FaceConstants.UserMask) >> (int)FaceConstants.UserShift) & 0xff;
                Hints = (ShapeHint)(materialId * FaceConstants.UserStep);
            }

            if ((hint & FaceConstants.ZBiasMask) != 0)
            {
                var bias = (hint & FaceConstants.ZBiasMask) / FaceConstants.ZBiasMask;
                Spec |= (int) FaceConstants.ZBiasMask * bias;
            }
        }
        
        var skipPoly = true;
        foreach (var vertex in Vertices)
        {
            var srcPoint = LOD.Points[vertex.PointIndex];
            var srcNormal = LOD.Normals[vertex.NormalIndex];
            if (!srcPoint.Hidden && skipPoly)
            {
                skipPoly = false;
            }

            //TODO: Maybe this should be referenced
            var pointFlags = srcPoint.Hints;

            if (Hints != ShapeHint.ClipNone)
            {
                var userFlags = pointFlags & ShapeHint.UserMask;
                if (userFlags switch
                    {
                        ShapeHint.ClipNone => null,
                        ShapeHint.ShineLightHints => (uint?) 20,
                        ShapeHint.AmbientLightHints => (uint?) 40,
                        _ => (uint?) 0
                    } is { } offset)
                {
                    pointFlags &= ~ShapeHint.UserMask;
                    pointFlags |= Hints + offset + (uint)ShapeHint.UserStep;
                }
                else
                {
                    pointFlags |= Hints;
                }
            }

            var fogFlags = pointFlags & ShapeHint.FogMask;
            if (fogFlags is ShapeHint.FogSky or ShapeHint.FogDisable) pointFlags &= ~ShapeHint.ClipBack;

            MinimumMaterial = (int) ((uint)(pointFlags & ShapeHint.UserMask) / (uint)ShapeHint.UserStep);

            if (
                float.IsFinite(srcNormal.X) || !(Math.Abs(srcNormal.X) < 1.5) ||
                float.IsFinite(srcNormal.Y) || !(Math.Abs(srcNormal.Y) < 1.5) ||
                float.IsFinite(srcNormal.Z) || !(Math.Abs(srcNormal.Z) < 1.5)
            )
            {
                //Warn:: "Face %d, point %d, face points %d,%d,%d - invalid normal %g,%g,%g"

                srcNormal = LOD.Normals[vertex.NormalIndex] = Vector3.UnitY;
            } else if (srcNormal.LengthSquared() < MinimumNormal)
            {
                //Warn:: "Warning: %s:%s Face %d, point %d, face points %d,%d,%d - very small normal %g,%g,%g (less than {MinimumNormal})"
            }
            
            
            //TODO: ln 1109 & 1209

            var landFlags = pointFlags & ShapeHint.LandMask;
            if (landFlags == ShapeHint.LandOn) Spec |= (uint)(FaceRemarks.OnSurface | FaceRemarks.NoZWrite);
        }
        
        

    }
}