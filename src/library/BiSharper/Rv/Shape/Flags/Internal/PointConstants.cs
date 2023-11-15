namespace BiSharper.Rv.Shape.Flags.Internal;

public static class PointConstants
{
    public const uint OnLand = 0x1, UnderLand = 0x2, AboveLand = 0x4, KeepLand = 0x8/*, landMask = 0xf*/;
    public const uint Decal = 0x100, VerticalDecal = 0x200/*, decalMask = 0x300*/;
    public const uint NoLight = 0x10, AmbientLight = 0x20, FullLight = 0x40, HalfLight = 0x80/*, lightMask = 0xf0*/;
    public const uint NoFog = 0x1000, SkyFog = 0x2000/*, fogMask = 0x3000*/;
    public const uint UserMask = 0xff0000, UserStep = 0x010000;
    public const uint SpecialMask = 0xf000000, HiddenSpecial = 0x1000000;
    public const uint All = OnLand | UnderLand | AboveLand | KeepLand | Decal | VerticalDecal | NoLight | FullLight |
                            HalfLight | AmbientLight | NoFog | SkyFog | UserMask | SpecialMask;
}