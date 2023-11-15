using System.Numerics;
using System.Text;
using BiSharper.Common.IO;
using BiSharper.Rv.Shape.Flags;

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

    public static TaggType ReadTag(BinaryReader reader, out string? taggText, out int tagLength)
    {
        var startingTagg = reader.ReadBoolean();
        taggText = startingTagg ? reader.ReadAsciiZ() : Encoding.ASCII.GetString(reader.ReadBytes(64)).TrimEnd(char.MinValue);
        tagLength = reader.ReadInt32();
        
        if (taggText.Equals("#Mass#", StringComparison.OrdinalIgnoreCase)) return TaggType.Mass;
        if (taggText.Equals("#EndOfFile#", StringComparison.OrdinalIgnoreCase)) return TaggType.EndOfFile;
        if (taggText.Equals("#Animation#", StringComparison.OrdinalIgnoreCase)) return TaggType.Animation;
        if (taggText.Equals("#UVSet#", StringComparison.OrdinalIgnoreCase)) return TaggType.UVSet;
        if (taggText.Equals("#Property#", StringComparison.OrdinalIgnoreCase)) return TaggType.Property;
        if (taggText.Equals("#MaterialIndex#", StringComparison.OrdinalIgnoreCase)) return TaggType.MaterialIndex;
        return !taggText.StartsWith('#') ? TaggType.NamedSelection : TaggType.Unknown;
    }
    
}