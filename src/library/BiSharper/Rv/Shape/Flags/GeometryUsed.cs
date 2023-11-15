namespace BiSharper.Rv.Shape.Flags;

public enum GeometryUsed
{
    NoNormals = 1,
    NoXY = 2,
    NoNormalsOrXY = NoNormals | NoXY
}