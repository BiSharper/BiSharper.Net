using System.Numerics;

namespace BiSharper.Rv.Shape.Utils;

public static class ReadingUtilities
{
    public static Vector3 ReadVector3(this BinaryReader reader) => 
        new(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
}