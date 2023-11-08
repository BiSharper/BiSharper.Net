using System.Buffers;
using BiSharper.Common.IO.Compression;
using BiSharper.Rv.Bank.Models;

namespace BiSharper.Rv.Bank;

public partial class FileBank
{
    public async Task<byte[]?> ReadRawAsync(EntryMeta meta, CancellationToken cancellationToken = default)
    {
        await _readLock.WaitAsync(cancellationToken);
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
            var foundBytes = await _input.ReadAsync(memory, cancellationToken); // using asynchronous read
            if (foundBytes != bufferSize)
            {
                throw new IOException($"Expected enough room for {bufferSize} (+4 for signed) bytes at position {_input.Position} but could only read {foundBytes}.");
            }

            return memory.Span[..bufferSize].ToArray();
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
            _readLock.Release();
        }
    }
    
    public async Task<byte[]?> ReadRawAsync(string name, CancellationToken cancellationToken = default) => GetMetadata(name) is not { } meta ? null : await ReadRawAsync(meta, cancellationToken);
    
    public async Task<byte[]?> ReadAsync(string name, CancellationToken cancellationToken = default) => GetMetadata(name) is not { } meta ? null : await ReadAsync(meta, cancellationToken);
    
    public async Task<byte[]?> ReadAsync(EntryMeta meta, CancellationToken cancellationToken = default)
    {
        var rentedBuffer = await ReadRawAsync(meta, cancellationToken); 
        if (rentedBuffer == null || rentedBuffer.Length == 0)
        {
            return rentedBuffer;
        }

        try
        {
            switch (meta.Mime)
            {
                case EntryMime.Decompressed:
                    return rentedBuffer;
                case EntryMime.Compressed:
                {
                    var goal = rentedBuffer.Length;
                    return BisCompatableLZSS.Decode(rentedBuffer, out var decompressed, (uint)goal) != goal ? rentedBuffer : decompressed;
                }
                case EntryMime.Encrypted:
                    throw new Exception("Encrypted entries may not be read!");
                case EntryMime.Version:
                    throw new Exception("Version entries may not be read!");
                default:
                    throw new Exception("Unknown entry may not be read!");
            }
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(rentedBuffer);
        }
        
    }
    
}