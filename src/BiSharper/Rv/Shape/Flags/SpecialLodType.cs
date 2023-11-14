namespace BiSharper.Rv.Shape.Flags;

public static class SpecialLodType
{
    private const float SpecialLod = 1e15f;
    public const float GeometryLod = 1e13f;
    public const float MemoryLod = SpecialLod * 1;
    public const float LandContactLod = SpecialLod * 2;
    public const float RoadwayLod = SpecialLod * 3;
    public const float PathsLod = SpecialLod * 4;
    public const float HitpointLod = SpecialLod * 5;
    
}