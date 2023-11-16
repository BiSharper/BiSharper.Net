namespace BiSharper.Rv.Texture.Models;

public struct RvSurfaceInfo
{
    /// filename only mask for texture matching
    public string NameMask { get; init; }
    /// full wild-card pattern mask for matching
    public string PatternMask { get; init; }
    /// name used for loading
    public string EntryName { get; init; }
    /// ID used for scripting functions
    public string Type { get; init; }
    
    /// <summary>
    /// Describes the irregularity of the surface
    /// </summary>
    public float Roughness { get; init; }
    /// <summary>
    /// The amount of dust generated from the surface
    /// </summary>
    public float DustProduction { get; init; }
    /// <summary>
    /// How much the speed of a bullet is reduced for 1 meter of distance travelled through the surface.
    /// </summary>
    public float BulletPenetrationCapacity { get; init; }
    /// <summary>
    /// The thickness of the surface
    /// </summary>
    public float SurfaceThickness { get; init; }
    /// <summary>
    /// Determines if surface is water, used for floating.
    /// </summary>
    public bool IsWater { get; init; }
    public string SoundEnvironmentType { get; init; }
    public string ImpactType { get; init; }
    public RvSurfaceCharacter Character { get; init; }
    public RvTextureType TextureType { get; init; }

    
    
}