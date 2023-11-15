using System.Text;

namespace BiSharper.Common.IO;

public static class FormatReader
{
    public static string ReadAsciiZ(this BinaryReader reader, int maxLength = 1024)
    {
        Span<byte> byteBuffer = stackalloc byte[maxLength];
        var index = 0;

        while (true)
        {
            var b = reader.ReadByte();

            if (b == 0)
                break;
            if(index >= byteBuffer.Length) 
                throw new Exception("Read string is longer than buffer. Increase the buffer size or handle this case.");
            

            byteBuffer[index] = b;
            index++;
        }

        return Encoding.ASCII.GetString(byteBuffer[..index]);
    }

    
}