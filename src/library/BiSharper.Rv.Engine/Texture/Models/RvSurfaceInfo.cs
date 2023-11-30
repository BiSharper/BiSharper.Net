using BiSharper.Rv.Param.Common;
using BiSharper.Rv.Render.Texture.Models.Clutter;
using BiSharper.Rv.Sound;

namespace BiSharper.Rv.Render.Texture.Models;

public class RvSurfaceInfo
{
    public readonly ParamContext Context;

    /// filename only mask for texture matching
    public string? NameMask { get; } = null;
    /// full wild-card pattern mask for matching
    public string? PatternMask { get; } = null;
    /// name used for loading
    public string EntryName => Context.ContextName;

    /// ID used for scripting functions
    public string? Type => Context.ContextName;


    /// <summary>
    /// How much the speed of a bullet is reduced for 1 meter of distance travelled through the surface.
    /// </summary>
    public float BulletPenetrationCapacity { get; } = 0;

    /// <summary>
    /// Factor used on roadways; Determines how much you want to reduce ground clutter on terrain?
    /// </summary>
    public float TerrainClutterFactor { get; } = 0;

    /// <summary>
    /// The thickness of the surface
    /// </summary>
    public float SurfaceThickness { get; }

    /// <summary>
    /// Determines if surface is water, used for floating.
    /// </summary>
    public bool IsWater => Context.GetString("isWater") == "true" || SoundEnvironment == "water" ;

    // public float Audibility => Context.GetFloat("audibility") ?? 1;
    /// <summary>
    /// Describes the irregularity of the surface
    /// </summary>
    // public float Roughness => Context.GetFloat("rough") ?? 0;
    /// <summary>
    /// The amount of dust generated from the surface
    /// </summary>
    // public float DustProduction => Context.GetFloat("dust") ?? 0;
    public string? SoundEnvironment => Context.GetString("soundEnviron");
    public string? ImpactType => Context.GetString("impact");
    public Dictionary<RvVisiblePose, float> Visibility = new();

    public RvSurfaceCharacter Character { get; }
    public HitSoundType HitSound { get; }
    public List<float> SurfaceSounds { get; }



    public RvSurfaceInfo(ParamContext context)
    {
        Context = context;
        if (context.GetClass("visibility") is not { } visibility) return;
        // Visibility[RvVisiblePose.Down] = visibility.GetFloat("down") ?? 1;
        // Visibility[RvVisiblePose.Prone] = visibility.GetFloat("prone") ?? 1;
        // Visibility[RvVisiblePose.Kneel] = visibility.GetFloat("kneel") ?? 1;
        // Visibility[RvVisiblePose.Stand] = visibility.GetFloat("stand") ?? 1;

    }

    public float GetSurfaceSound(RvSurfaceSound sound) =>
        SurfaceSounds.Count > (int)sound ? SurfaceSounds[(int)sound] : 0.0f;

}

