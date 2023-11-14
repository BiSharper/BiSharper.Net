using System.Numerics;
using System.Text;
using BiSharper.Common.IO;
using BiSharper.Rv.Shape.Flags;

namespace BiSharper.Rv.Shape.Types;

public readonly struct ShapeFace
{
    public DetailLevel LOD { get; private init; }
    public Vertex[] Vertices { get; private init; }
    public string Texture { get; private init; }
    public string? Material { get; private init; }
    public ShapeHint Hints { get; private init; }
    public uint Spec { get; private init; }
    public int MinimumMaterial { get; private init; }
    public static readonly float MinimumNormal = (float)Math.Sqrt(0.1f);

    public readonly struct Vertex
    {
        public ShapeFace Face { get; private init; }
        public int PointIndex { get; private init; }
        public int NormalIndex { get; private init; }
        public float X { get; private init; }
        public float Y { get; private init; }

        public Vertex(BinaryReader reader, ShapeFace face)
        {
            const float xyLimit = 100.0f;
            Face = face;
            PointIndex = reader.ReadInt32();
            NormalIndex = reader.ReadInt32();
            float x = reader.ReadSingle(), y = reader.ReadSingle();
            float absX = Math.Abs(x), absY = Math.Abs(y);
            if (absX >= 1e5 || absY >= 1e5 || !float.IsFinite(absX) || !float.IsFinite(absY))
            {
                //Warn:: "Face %d, point %d, face points %d,%d,%d - invalid uv %g,%g"
                x = y = 0;
            }

            
            if (absX > xyLimit || absY > xyLimit)
            {
                //Warn::  "UV coordinate on point %d is too big UV(%f, %f) - the UV compression may produce inaccurate results"
            }

            X = x;
            Y = y;
        }

        public static Vertex[] ReadMulti(BinaryReader reader, ShapeFace face, int count)
        {
            var vertices = new Vertex[count];
            for (var i = 0; i < count; i++) vertices[i] = new Vertex(reader, face);
            return vertices;
        }
    }
    
    public static ShapeFace[] ReadMulti(BinaryReader reader, bool extended, bool material, DetailLevel parent, int count)
    {
        var faces = new ShapeFace[count];
        for (var i = 0; i < count; i++) faces[i] = new ShapeFace(reader, extended, material, parent);
        return faces;
    }

    [Flags]
    private enum FaceRemarks : uint
    {
        SunPrecalculated = 1,
        OnSurface = 2,
        IsOnSurface = 4,
        NoZBuf = 8,
        NoZWrite = 0x10,
        NoShadow = 0x20,
        IsShadow = 0x40,
        NoAlphaWrite = 0x80,
        IsAlpha = 0x100,
        IsTransparent = 0x200,
        IsShadowVolume = 0x400,
        IsLight = 0x800,
        DstBlendOne = IsLight,
        ShadowVolumeFrontFaces = 0x1000,
        NoBackfaceCull = ShadowVolumeFrontFaces,
        ClamLog = 14,
        ClampMask = 3,
        NoClamp = 0x2000,
        ClampX = 0x4000,
        ClampY = 0x8000,
        IsAnimated = 0x10000,
        IsAlphaOrdered = 0x20000,
        NoColorWrite = 0x40000,
        IsAlphaFog = 0x80000,
        DstBlendZero=0x100000,
        IsColored=0x200000,
        IsHidden=0x400000,
        BestMipmap=0x800000, 
        FilterMask =0x3000000,
        FilterTrilinear=0x0000000, 
        FilterLinear=0x1000000,
        FilterAnisotropic=0x2000000,
        FilterPoint=0x3000000,
        ZBiasMask=0xc000000,
        ZBiasStep=0x4000000,
        IsHiddenProxy=0x10000000, 
        NoStencilWrite=0x20000000,
        TracerLighting=0x40000000,
        DisableSun=0x80000000,
        
        
    }

    public ShapeFace(BinaryReader reader, bool extended, bool material, DetailLevel parent)
    {
        const uint noLight = 0x1, ambientLight = 0x2, fullLight = 0x4, biSidedLight = 0x20, skyLight = 0x80,
            reverseLight = 0x100000, flatLight = 0x200000, lightMask = 0x3000a7;
        const uint shadow = 0x8, noShadow = 0x10, shadowMask = 0x18,
            zBiasMask = 0x300, zBiasStep = 0x100, colorizeMask = 0xf000, fanStripMask = 0xf0000,
            beginFan = 0x10000, beginStrip = 0x20000, continueFan = 0x40000, continueStrip = 0x80000,
            disableTexMerge = 0x1000000, userMask = 0xfe000000, userStep = 0x02000000, userShift = 25;
        const uint all = noLight | ambientLight | fullLight | biSidedLight | skyLight | reverseLight |
                         flatLight | shadow | noShadow | disableTexMerge | userMask | zBiasMask |
                         colorizeMask | fanStripMask;
        LOD = parent;
        Hints = 0;
        uint hint;
        switch (material)
        {
            case true:
            {
                Vertices = Vertex.ReadMulti(reader, this, reader.ReadInt32());
                hint = (uint) reader.ReadInt32();
                Texture = reader.ReadAsciiZ().ToLower(); //RString::ReadString(QIStream &in)
                Material = reader.ReadAsciiZ().ToLower(); //RString::ReadString(QIStream &in)
                break;
            }
            case false:
            {
                Texture = Encoding.ASCII.GetString(reader.ReadBytes(32)).TrimEnd('\0').ToLower();
                Vertices = Vertex.ReadMulti(reader, this, reader.ReadInt32());
                Material = null;
                hint = extended ? (uint) reader.ReadInt32() : 0;
                break;
            }
        }

        if ((hint & all) != 0)
        {
            if ((hint & noLight) != 0) Hints |= ShapeHint.ShineLightHints;
            else if ((hint & ambientLight) != 0) Hints |= ShapeHint.AmbientLightHints;
            else if ((hint & fullLight) != 0) Hints |= ShapeHint.FullLightHints;
            
            if ((hint & shadow) != 0) Spec |= 0x40;
            if ((hint & noShadow) != 0) Spec |= 0x20;


            if ((hint & userMask) != 0)
            {
                var materialId = ((hint & userMask) >> (int)userShift) & 0xff;
                Hints = (ShapeHint)(materialId * (uint)ShapeHint.UserStep);
            }

            if ((hint & zBiasMask) != 0)
            {
                var bias = (hint & zBiasMask) / zBiasStep;
                Spec |= (int) zBiasStep * bias;
            }
        }
        
        var minMaterial = int.MaxValue;//TODO: shape might have to own this as a property/field
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