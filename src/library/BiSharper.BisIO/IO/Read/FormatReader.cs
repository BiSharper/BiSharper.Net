using System.Text;

namespace BiSharper.BisIO.IO.Read;

public static class FormatReader
{
    public static string ReadAsciiZ(this BinaryReader reader, int maxLength = 1024)
    {
        Span<byte> byteBuffer = stackalloc byte[maxLength];
        var index = 0;

        while (reader.ReadByte() is var b && b != 0)
        {
            if (byteBuffer.Length <= index)
                throw new Exception("Read string is longer than buffer. Increase the buffer size or handle this case.");
            byteBuffer[index++] = b;
        }

        return Encoding.ASCII.GetString(byteBuffer[..index]);
    }

    
}