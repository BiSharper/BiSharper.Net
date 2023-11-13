namespace BiSharper.Rv.Shape.Flags;

[Flags]
public enum FaceRemark
{
    NoLight = 0x1,
    AmbientLight = 0x2,
    FullLight = 0x4,
    BiSidedLight = 0x20,
    SkyLight = 0x80,
    ReverseLight = 0x100000,
    FlatLight = 0x200000,
    LightMask = 0x3000a7,
    DisableTexMerge = 0x1000000,
    Shadow = 0x8,
    NoShadow = 0x10,
    ShadowMask = 0x18,
}