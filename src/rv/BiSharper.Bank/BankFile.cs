using System.Text;
using BiSharper.FileBank.Models;

namespace BiSharper.FileBank;

public class Bank
{
    private readonly Dictionary<string, string> _properties = new();
    private readonly Dictionary<string, EntryMeta> _dataEntries = new();

    private Bank()
    {
        
    }

    public Bank(Dictionary<string, string> properties, Dictionary<string, EntryMeta> dataEntries)
    {
        _properties = properties;
        _dataEntries = dataEntries;
    }

    public Bank(BinaryReader reader) => ReadBank(reader);

    private void ReadBank(BinaryReader reader, bool calculateOffsets = true, bool readPropertiesOnInnerVersion = false, bool breakOnInnerVersion = true)
    {
        ReadEntryList(reader, _properties, _dataEntries, calculateOffsets, readPropertiesOnInnerVersion, breakOnInnerVersion);
        
        

    }

    private static void ReadEntryList(
        BinaryReader reader,
        IDictionary<string, string> properties,
        IDictionary<string, EntryMeta> dataEntries,
        bool calculateOffsets = true,
        bool readPropertiesOnInnerVersion = false,
        bool breakOnInnerVersion = true
    )
    {
        var entries = new Dictionary<string, EntryMeta>();
        var offset = (int)reader.BaseStream.Position;
        var first = true;
        for (;;)
        {
            var (entryName, entryMeta) = ReadEntry(reader);
            if (calculateOffsets)
            {
                entryMeta.Offset = (ulong) offset;
                offset += (int)entryMeta.BufferLength;
            }

            if (entryName.Length > 0)
            {
                entryName = entryName.ToLower().Replace('/', '\\');
                entries[entryName] = entryMeta;
            }
            else if(entryMeta is { Mime: EntryMime.Version, BufferLength: 0, Timestamp: 0 })
            {
                if (first || readPropertiesOnInnerVersion) ReadVersionProperties(reader, properties);
                if(breakOnInnerVersion) break;
                continue;
            }
            else
            {
                break;
            }
            
            first = false;
        }
        var headerEnd = (uint)reader.BaseStream.Position;

        foreach (var (name, meta) in entries)
        {
            dataEntries[name] = calculateOffsets ? meta with { Offset = meta.Offset + headerEnd } : meta;
        }
    }


    private static void ReadVersionProperties(BinaryReader reader, IDictionary<string, string> properties)
    {
        for (;;)
        {
            var name = ReadEntryName(reader);
            if(name.Length == 0) break;
            var value = ReadEntryName(reader);

            if (name.Equals("prefix", StringComparison.OrdinalIgnoreCase))
            {
                name = "prefix";
                value = value + '\\';
            }
            properties.Add(name, value);
        }
    }
    
    private static (string, EntryMeta) ReadEntry(BinaryReader reader) => 
    (
        ReadEntryName(reader),
        new EntryMeta
        {
            Mime = (EntryMime)reader.ReadInt32(),
            Length = (uint)reader.ReadInt32(),
            Offset = (ulong)reader.ReadInt32(),
            Timestamp = (uint)reader.ReadInt32(),
            BufferLength = (uint)reader.ReadInt32()
        }
    );
    

    private static string ReadEntryName(BinaryReader reader)
    {
        Span<byte> buffer = stackalloc byte[1024];

        int i;
        for (i = 0; i < buffer.Length; i++)
        {
            var current = reader.ReadByte();
            if(current == 0) break;
        
            buffer[i] = current;
        }
        
        return Encoding.UTF8.GetString(buffer.Slice(0, i));
    }
}