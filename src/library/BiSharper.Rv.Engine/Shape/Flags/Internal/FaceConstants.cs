namespace BiSharper.Rv.Render.Shape.Flags.Internal;

public static class FaceConstants
{
    public const uint NoLight = 0x1, AmbientLight = 0x2, FullLight = 0x4, BiSidedLight = 0x20, SkyLight = 0x80,
        ReverseLight = 0x100000, FlatLight = 0x200000, LightMask = 0x3000a7;
    public const uint Shadow = 0x8, NoShadow = 0x10, ShadowMask = 0x18,
        ZBiasMask = 0x300, ZBiasStep = 0x100, ColorizeMask = 0xf000, FanStripMask = 0xf0000,
        BeginFan = 0x10000, BeginStrip = 0x20000, ContinueFan = 0x40000, ContinueStrip = 0x80000,
        DisableTexMerge = 0x1000000, UserMask = 0xfe000000, UserStep = 0x02000000, UserShift = 25;
    public const uint All = NoLight | AmbientLight | FullLight | BiSidedLight | SkyLight | ReverseLight |
                           FlatLight | Shadow | NoShadow | DisableTexMerge | UserMask | ZBiasMask |
                           ColorizeMask | FanStripMask;
}