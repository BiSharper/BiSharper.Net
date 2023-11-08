using System.Buffers;
using System.Collections.Concurrent;
using BiSharper.Common.IO.Compression;
using BiSharper.Rv.Bank.Models;

namespace BiSharper.Rv.Bank;

public partial class FileBank
{
    private readonly Stream _input;
    private readonly long _binaryLength;
    private readonly SemaphoreSlim _readLock = new(1, 1);
    private readonly ConcurrentDictionary<string, string> _properties = new();
    private readonly ConcurrentDictionary<string, BankEntry> _dataEntries = new();
    public IEnumerable<KeyValuePair<string, string>> Properties => _properties;
    public IEnumerable<KeyValuePair<string, BankEntry>> DataEntries => _dataEntries;

    public string DefaultPrefix { get; }
    
    public string? GetProperty(string name) => _properties.GetValueOrDefault(name);

    public string Prefix => GetProperty("prefix") ?? DefaultPrefix;

    public bool HasProperty(string name) => _properties.ContainsKey(name);

    public bool HasEntry(string name) => _dataEntries.ContainsKey(name);

    public BankEntry? GetMetadata(string name) => _dataEntries.GetValueOrDefault(name);
    
    public byte[]? ReadRaw(string name) => GetMetadata(name) is not { } meta ? null : ReadRaw(meta);
    
    public byte[]? ReadRaw(BankEntry meta)
    {
        _readLock.Wait();
        var buffer = Array.Empty<byte>();
        try
        {
            if (meta.Offset > _binaryLength)
            {
                return null;
            }

            if (meta.BufferLength == 0)
            {
                return buffer;
            }
            
            _input.Seek(meta.Offset, SeekOrigin.Begin);
            var bufferSize = (int)meta.BufferLength;
            buffer = ArrayPool<byte>.Shared.Rent(bufferSize);
            var memory = new Memory<byte>(buffer, 0, bufferSize);
            var foundBytes = _input.Read(buffer);
            if (foundBytes != bufferSize)
            {
                throw new IOException($"Expected enough room for {bufferSize} (+4 for signed) bytes at position {_input.Position} but could only read {foundBytes}.");
            }
            var result = new byte[bufferSize];
            memory.Span[..bufferSize].CopyTo(result);
            return result;
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
            _readLock.Release();
        }
    }
    
    public byte[]? Read(string name) => GetMetadata(name) is not { } meta ? null : Read(meta);
    
    public byte[]? Read(BankEntry meta)
    {
        var buffer = ReadRaw(meta);
        if (buffer == null || buffer.Length == 0)
        {
            return buffer;
        }

        try
        {
            byte[] result;
            switch (meta.Mime)
            {
                case EntryMime.Decompressed:
                    result = buffer;
                    break;
                case EntryMime.Compressed:
                {
                    var goal = buffer.Length;
                    result = BisCompatableLZSS.Decode(buffer, out var decompressed, (uint)goal) != goal ? buffer : decompressed;
                    break;
                }
                case EntryMime.Encrypted:
                    throw new Exception("Encrypted entries may not be read!");
                case EntryMime.Version:
                    throw new Exception("Version entries may not be read!");
                default:
                    throw new Exception("Unknown entry may not be read!");
            }
            return result;
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }

    }
}