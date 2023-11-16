using System.Diagnostics.CodeAnalysis;

namespace BiSharper.Rv.Shape.Flags.Internal;

[Flags]
[SuppressMessage("ReSharper", "UnusedMember.Local")]
internal enum FaceRemarks : uint
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