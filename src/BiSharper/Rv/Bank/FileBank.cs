using System.Collections.Concurrent;
using BiSharper.Common.IO.Compression;
using BiSharper.Rv.Bank.Models;

namespace BiSharper.Rv.Bank;

public partial class FileBank
{
    private readonly Stream _input;
    private readonly long _binaryLength;
    private readonly ConcurrentDictionary<string, string> _properties = new();
    private readonly ConcurrentDictionary<string, EntryMeta> _dataEntries = new();
    public string DefaultPrefix { get; }
    
    public string? GetProperty(string name) => _properties.GetValueOrDefault(name);

    public bool HasProperty(string name) => _properties.ContainsKey(name);

    public bool HasEntry(string name) => _dataEntries.ContainsKey(name);

    public EntryMeta? GetMetadata(string name) => _dataEntries.GetValueOrDefault(name);
    
    public byte[]? ReadRaw(string name) => GetMetadata(name) is not { } meta ? null : ReadRaw(meta);
    
    public byte[]? ReadRaw(EntryMeta meta)
    {
        lock (_input)
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
}