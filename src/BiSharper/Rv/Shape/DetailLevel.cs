using BiSharper.Rv.Shape.Types;

namespace BiSharper.Rv.Shape;

public readonly struct DetailLevel
{
    public LODShape Shape { get; private init; }
    public List<ShapeFace> Faces { get; private init; }
    
    public DetailLevel(BinaryReader reader, LODShape parent)
    {
        Shape = parent;
        
        var signature = reader.ReadBytes(4);
        var headSize = reader.ReadInt32();
        var version = reader.ReadInt32();
        var posCount = reader.ReadInt32();
        var normalCount = reader.ReadInt32();
        var faceCount = reader.ReadInt32();
        var flags = reader.ReadInt32();
        
        bool extended = false, material = false;
        if (signature == "P3DM"u8 || signature == "SP3X"u8)
        {
            extended = true;
            material = true;

            if (version != 0x100 && signature[0] == 'P')
            {
                throw new NotSupportedException($"Unsupported P3DM version {version}.");
            }

            reader.BaseStream.Seek(headSize - 28, SeekOrigin.Current);
        } else if (signature == "SP3D"u8)
        {
            //Our variables are all out of wack in accordance to this format - lets fix that.
            posCount = headSize;
            headSize = 16;
            normalCount = version;
            version = 0;
            faceCount = posCount;
            flags = 0;
        }
        Faces = new List<ShapeFace>(faceCount);




    }
    
}