using System.Text;
using BiSharper.Common.IO;
using BiSharper.Rv.Shape.Flags;

namespace BiSharper.Rv.Shape.Types;

public readonly struct ShapeFace
{
    public DetailLevel LOD { get; private init; }
    public List<ShapeVertex> Vertices { get; private init; }
    public string Texture { get; private init; }
    public string? Material { get; private init; }
    public FaceRemark Remarks { get; private init; }


    public ShapeFace(BinaryReader reader, bool extended, bool material, DetailLevel parent)
    {
        LOD = parent;
        switch (material)
        {
            case true:
            {
                //TODO: **Start** Read Vertices
                Vertices = new List<ShapeVertex>(reader.ReadInt32());
                //TODO: **End** 
                
                Remarks = (FaceRemark) reader.ReadInt32();
                
                Texture = reader.ReadAsciiZ().ToLower(); //RString::ReadString(QIStream &in)
                Material = reader.ReadAsciiZ().ToLower(); //RString::ReadString(QIStream &in)
                break;
            }
            case false:
            {
                Texture = Encoding.ASCII.GetString(reader.ReadBytes(32)).TrimEnd('\0').ToLower();
                        
                //TODO: **Start** Read Vertices
                Vertices = new List<ShapeVertex>(reader.ReadInt32());
                //TODO: **End** 
                Material = null;
                
                Remarks = extended ? (FaceRemark) reader.ReadInt32() : 0;
                break;
            }
        }

    }
}