namespace BiSharper.Rv.Shape.Flags;

[Flags]
public enum ShapeRemark
{
    Normal = 0,
    Reversed = 1,
    Shadow = 2,
    Animated = 4,
    SeparateShadowFaces = 8,
    NotAnimated = 16,
    OnSurface = 32,
    Modified = 64
}