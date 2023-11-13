using BiSharper.Rv.Shape.Flags;

namespace BiSharper.Rv.Shape.Types;

public struct ShapeFace
{
    public List<ShapeVertex> Vertices { get; private set; }
    public string Texture { get; private set; }
    public string? Material { get; private set; }
    public FaceRemark Remarks { get; private set; }
    
    
}