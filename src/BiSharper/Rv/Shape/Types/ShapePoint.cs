using BiSharper.Rv.Shape.Flags;

namespace BiSharper.Rv.Shape.Types;

public readonly struct ShapePoint
{
    public DetailLevel LOD { get; private init; }
    public float X { get; private init; }
    public float Y { get; private init; }
    public float Z { get; private init; }
    public bool Hidden { get; private init; }
    public VertexRemark VertexRemarks { get; private init; }
    public PointRemark Remarks { get; private init; }

    public ShapePoint(BinaryReader reader, bool extended, DetailLevel parent)
    {
        LOD = parent;
        switch (extended)
        {
            case true:
            {
                break;
            }
            case false:
            {
                
                break;
            }
        }

    }

}