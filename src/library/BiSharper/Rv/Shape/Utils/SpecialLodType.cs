using BiSharper.Rv.Shape.Flags;

namespace BiSharper.Rv.Shape.Utils;

public static class SpecialLodType
{
    public const float Tolerance = 1e-3f;
    private const float SpecialLod = 1e15f;
    public const float GeometryLod = 1e13f;
    public const float MemoryLod = SpecialLod * 1;
    public const float LandContactLod = SpecialLod * 2;
    public const float RoadwayLod = SpecialLod * 3;
    public const float PathsLod = SpecialLod * 4;
    public const float HitpointsLod = SpecialLod * 5;
    public const float ViewGeometryLod = SpecialLod * 6; 
    public const float FireGeometryLod = SpecialLod * 7;
    public const float ViewCargoGeometryLod = SpecialLod * 8;
    public const float ViewPilotGeometryLod = SpecialLod * 13; 
    public const float ViewGunnerGeometryLod = SpecialLod * 15; 
    public const float FireGunnerGeometryLod = SpecialLod * 16; 
    public const float SubPartsLod = SpecialLod * 17;
    public const float ShadowVolumeCargoLod = SpecialLod * 18;
    public const float ShadowVolumePilotLod = SpecialLod * 19;
    public const float ShadowVolumeGunnerLod = SpecialLod * 20;
    public const float WreckLod = SpecialLod * 21; 
    public const float ShadowVolumeLod = 10000f;
    private const float ShadowVolumeEnd = 10999f;
    public const float ShadowBufferLod = 11000f;
    private const float ShadowBufferEnd = 11999f;
    public static readonly float[] GeometryLods = 
    {
        GeometryLod, MemoryLod, LandContactLod, RoadwayLod, PathsLod, 
        HitpointsLod, ViewGeometryLod, FireGeometryLod, 
        ViewPilotGeometryLod, ViewCargoGeometryLod, ViewGunnerGeometryLod
    };
    
    public static GeometryUsed ResolveGeometryUsed(float resolution)
    {
        if (resolution <= 900)
        {
            if (GeometryLods.Any(lod => IsLod(resolution, lod)))
            {
                return GeometryUsed.NoNormalsOrXY;
            }

            if (IsShadowVolumeLod(resolution))
            {
                return GeometryUsed.NoXY;
            }

            if (WithinShadowBuffer(resolution))
            {
                return GeometryUsed.NoNormals;
            }
        }

        return GeometryUsed.Default;
    }

    public static bool IsLod(float resolution, float spec) => Math.Abs(resolution - spec) < spec * Tolerance;
    
    public static bool IsShadowVolumeLod(float resolution) =>
        WithinShadowVolume(resolution) ||
        IsLod(resolution, ShadowVolumeCargoLod) ||
        IsLod(resolution, ShadowVolumePilotLod) ||
        IsLod(resolution, ShadowVolumeCargoLod);

    public static bool WithinShadowVolume(float resolution) =>
        WithinSpec(resolution, ShadowVolumeLod, ShadowVolumeEnd);
   
   
    public static bool WithinShadowBuffer(float resolution) =>
       WithinSpec(resolution, ShadowBufferLod, ShadowBufferEnd);
    
    private static bool WithinSpec(float resolution, float spec, float specMax) => 
        resolution > spec - Tolerance && resolution < specMax + Tolerance;
}