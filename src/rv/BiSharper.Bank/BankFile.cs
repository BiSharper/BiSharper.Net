using System.Text;
using BiSharper.Common.Compression;
using BiSharper.FileBank.Models;

namespace BiSharper.FileBank;

public class Bank
{
    private readonly Stream _input;
    private readonly long _binaryLength;
    private readonly Dictionary<string, string> _properties = new();
    private readonly Dictionary<string, EntryMeta> _dataEntries = new();

    public Bank(
            Stream input,
            bool calculateOffsets = true,
            bool readPropertiesOnInnerVersion = false,
            bool breakOnInnerVersion = true,
            bool ignoreImpossibleOffsets = true,
            bool headerOffsetIsImpossible = true
        )
    {
        _input = input;
        _binaryLength = _input.Length;
        ReadEntryList(_input, _properties, _dataEntries, calculateOffsets, readPropertiesOnInnerVersion, breakOnInnerVersion, ignoreImpossibleOffsets, headerOffsetIsImpossible);
    }
    
    public string? GetProperty(string name) => _properties.GetValueOrDefault(name);

    public bool HasProperty(string name) => _properties.ContainsKey(name);

    public bool HasEntry(string name) => _dataEntries.ContainsKey(name);

    public EntryMeta? GetMetadata(string name) => _dataEntries.GetValueOrDefault(name);
    
    public byte[]? ReadRaw(string name) => GetMetadata(name) is not { } meta ? null : ReadRaw(meta);
    
    public byte[]? ReadRaw(EntryMeta meta)
    {
        if (meta.Offset > _binaryLength)
        {
            return null;
        }

        if (meta.BufferLength == 0)
        {
            return Array.Empty<byte>();
        }

        _input.Seek(meta.Offset, SeekOrigin.Begin);
        var bufferSize = (int)meta.BufferLength;
        var buffer = new byte[bufferSize];
        var foundBytes = _input.Read(buffer);
        if (foundBytes != bufferSize)
        {
            throw new IOException($"Expected enough room for {bufferSize} (+4 for signed) bytes at position {_input.Position} but could only read {foundBytes}.");
        }

        return buffer;
    }
    
    public byte[]? Read(string name) => GetMetadata(name) is not { } meta ? null : Read(meta);
    
    public byte[]? Read(EntryMeta meta)
    {
        if (ReadRaw(meta) is not { } raw)
        {
            return null;
        }

        if (raw.Length == 0)
        {
            return raw;
        }

        switch (meta.Mime)
        {
            case EntryMime.Decompressed:
                return raw;
            case EntryMime.Compressed:
            {
                var goal = raw.Length;
                return BisCompatableLZSS.Decode(raw, out var decompressed, (uint)goal) != goal ? raw : decompressed;
            }
            case EntryMime.Encrypted:
                throw new Exception("Encrypted entries may not be read!");
            case EntryMime.Version:
                throw new Exception("Version entries may not be read!");
            default:
                throw new Exception("Unknown entry may not be read!");
        }

    }
    

    
    private static void ReadEntryList(
        Stream input,
        IDictionary<string, string> properties,
        IDictionary<string, EntryMeta> dataEntries,
        bool calculateOffsets = true,
        bool readPropertiesOnInnerVersion = false,
        bool breakOnInnerVersion = true,
        bool ignoreImpossibleOffsets = true,
        bool headerOffsetIsImpossible = true,
        long? streamLength = null
    )
    {
        streamLength ??= input.Length;
        var entries = new Dictionary<string, EntryMeta>();
        var offset = (int)input.Position;
        var first = true;
        for (;;)
        {
            var (entryName, entryMeta) = ReadEntry(input);
            if (calculateOffsets)
            {
                entryMeta.Offset = (long) (ulong) offset;
                offset += (int)entryMeta.BufferLength;
            }

            if (entryName.Length > 0)
            {
                entryName = entryName.ToLower().Replace('/', '\\');
                entries[entryName] = entryMeta;
            }
            else if(entryMeta is { Mime: EntryMime.Version, BufferLength: 0, Timestamp: 0 })
            {
                if (first || readPropertiesOnInnerVersion) ReadVersionProperties(input, properties);
                if(breakOnInnerVersion) break;
                continue;
            }
            else
            {
                break;
            }
            
            first = false;
        }
        var headerEnd = (uint)input.Position;

        foreach (var (name, meta) in entries)
        {
            var correctedMeta = calculateOffsets ? meta with { Offset = meta.Offset + headerEnd } : meta;
            if (!(ignoreImpossibleOffsets && ((correctedMeta.Offset < headerEnd && headerOffsetIsImpossible) ||
                                              correctedMeta.Offset > streamLength|| 
                                              correctedMeta.Offset + correctedMeta.BufferLength >= streamLength)))
            {
                dataEntries[name] = correctedMeta;
            }
        }
    }


    private static void ReadVersionProperties(Stream input, IDictionary<string, string> properties)
    {
        for (;;)
        {
            var name = ReadEntryName(input);
            if(name.Length == 0) break;
            var value = ReadEntryName(input);

            if (name.Equals("prefix", StringComparison.OrdinalIgnoreCase))
            {
                name = "prefix";
                value += '\\';
            }
            properties.Add(name, value);
        }
    }
    
    private static (string, EntryMeta) ReadEntry(Stream input) => 
    (
        ReadEntryName(input),
        new EntryMeta
        {
            Mime = (EntryMime)TakeInt(input),
            Length = (uint)TakeInt(input),
            Offset = (long)(ulong)TakeInt(input),
            Timestamp = (uint)TakeInt(input),
            BufferLength = (uint)TakeInt(input)
        }
    );

    private static int TakeInt(Stream input)
    {
        Span<byte> buffer = stackalloc byte[sizeof(int)];
        var foundBytes = input.Read(buffer);
        if (foundBytes != sizeof(int))
        {
            throw new IOException($"Expected enough room for four bytes at position {input.Position} but could only read {foundBytes}.");
        }
        
        return BitConverter.ToInt32(buffer);
    }
    

    private static string ReadEntryName(Stream input)
    {
        Span<byte> buffer = stackalloc byte[1024];

        int i;
        for (i = 0; i < buffer.Length; i++)
        {
            var current = input.ReadByte();
            if(current <= 0) break;
        
            buffer[i] = (byte)current;
        }
        
        return Encoding.UTF8.GetString(buffer[..i]);
    }
}