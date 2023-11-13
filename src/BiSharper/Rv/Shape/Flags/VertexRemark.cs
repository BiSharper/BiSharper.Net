namespace BiSharper.Rv.Shape.Flags;

[Flags]
public enum VertexRemark
{
    ClipNone = 0,
    ClipFront = 1,
    ClipBack = 2,
    ClipLeft = 4,
    ClipRight = 8,
    ClipBottom = 16,
    ClipTop = 32,
    ClipAll = ClipFront | ClipBack | ClipLeft | ClipRight | ClipBottom | ClipTop,
    
    LandMask = 0xf00, LandStep = 0x100,
    LandNone = LandStep * 0,
    LandOn = LandStep * 1,
    LandUnder = LandStep * 2,
    LandAbove = LandStep * 4,
    LandKeep = LandStep * 8,
    
    DecalMask = 0x3000, DecalStep = 0x1000,
    DecalNone = DecalStep * 0,
    DecalNormal = DecalStep * 1,
    DecalVertical = DecalStep * 2,
    
    FogMask = 0xc000, FogStep = 0x4000,
    FogNormal = FogStep * 0,
    FogDisable = FogStep * 1,
    FogSky = FogStep * 2,
    
    LightMask = 0xf0000, LightStep = 0x10000,
    LightNormal = LightStep * 0,
    LightLine = LightStep * 8,
    
    UserMask = 0xff00000, UserStep = 0x100000,
    UserMax = 0xff,
    
    ClipHints = LandMask | DecalMask | FogMask | LightMask | UserMask
}