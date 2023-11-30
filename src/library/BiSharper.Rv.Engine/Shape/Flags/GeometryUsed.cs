namespace BiSharper.Rv.Shape.Flags;

[Flags]
public enum GeometryUsed
{
    Default = 0,
    NoNormals = 1,
    NoXY = 2,
    NoNormalsOrXY = NoNormals | NoXY
}