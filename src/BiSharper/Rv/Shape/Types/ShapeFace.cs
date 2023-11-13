using System.Text;
using BiSharper.Common.IO;
using BiSharper.Rv.Shape.Flags;

namespace BiSharper.Rv.Shape.Types;

public readonly struct ShapeFace
{
    public DetailLevel LOD { get; private init; }
    public Vertex[] Vertices { get; private init; }
    public string Texture { get; private init; }
    public string? Material { get; private init; }
    public FaceRemark Remarks { get; private init; }

    public readonly struct Vertex
    {
        public ShapeFace Face { get; private init; }
        public int PointIndex { get; private init; }
        public int NormalIndex { get; private init; }
        public int X { get; private init; }
        public int Y { get; private init; }

        public Vertex(BinaryReader reader, ShapeFace face)
        {
            Face = face;
            PointIndex = reader.ReadInt32();
            NormalIndex = reader.ReadInt32();
            X = reader.ReadInt32();
            Y = reader.ReadInt32();
        }

        public static Vertex[] ReadMulti(BinaryReader reader, ShapeFace face, int count)
        {
            var vertices = new Vertex[count];
            for (var i = 0; i < count; i++) vertices[i] = new Vertex(reader, face);
            return vertices;
        }
    }
    
    public ShapeFace(BinaryReader reader, bool extended, bool material, DetailLevel parent)
    {
        LOD = parent;
        switch (material)
        {
            case true:
            {
                Vertices = Vertex.ReadMulti(reader, this, reader.ReadInt32());
                Remarks = (FaceRemark) reader.ReadInt32();
                Texture = reader.ReadAsciiZ().ToLower(); //RString::ReadString(QIStream &in)
                Material = reader.ReadAsciiZ().ToLower(); //RString::ReadString(QIStream &in)
                break;
            }
            case false:
            {
                Texture = Encoding.ASCII.GetString(reader.ReadBytes(32)).TrimEnd('\0').ToLower();
                Vertices = Vertex.ReadMulti(reader, this, reader.ReadInt32());
                Material = null;
                Remarks = extended ? (FaceRemark) reader.ReadInt32() : 0;
                break;
            }
        }

    }
}