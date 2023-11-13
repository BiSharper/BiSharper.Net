namespace BiSharper.Rv.Shape.Flags;

public enum MaterialSection
{
    /// simple emissivity - be careful with HDR, can mostly be seen during night time only
    Shining=200,
    /// no diffuse
    InShadow,
    /// half diffuse, normal ignored
    HalfLighted,
    /// full diffuse, normal ignored
    FullLighted,
    /// little diffuse, normal used
    Inside,
    /// color * 0.75, no diffuse
    InShadow75,
    /// color * 0.75, little diffuse
    Inside75,
    /// color * 0.50, no diffuse
    InShadow50,
    /// color * 0.50, little diffuse
    Inside50,
    /// auto-adjusting emissivity - provides reasonable luminance both day and night
    ShiningAdjustable,
}