using System.Numerics;

namespace BiSharper.Rv.Shape.Utils;

public static class ReadingUtilities
{
    public static Vector3 ReadVector3(this BinaryReader reader) => 
        new(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

    public static Vector3[] ReadMultipleVector3(this BinaryReader reader, int count)
    {
        var vectors = new Vector3[count];
        for (var i = 0; i < count; i++) vectors[i] = reader.ReadVector3();
        return vectors;
    }
}