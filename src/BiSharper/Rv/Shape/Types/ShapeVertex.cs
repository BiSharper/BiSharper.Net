namespace BiSharper.Rv.Shape.Types;

public readonly struct ShapeVertex
{
    public required int PointIndex { get; init; }
    public required int NormalIndex { get; init; }
    public required int X { get; init; }
    public required int Y { get; init; }
}