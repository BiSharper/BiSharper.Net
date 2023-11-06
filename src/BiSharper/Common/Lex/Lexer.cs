using System.Text;

namespace BiSharper.Common.Lex;

public sealed partial class Lexer : IDisposable
{
    private readonly Stream _stream;
    private readonly int _maxBufferReadLength;
    private readonly char[] _buffer;
    private readonly Encoder _encoder;
    private bool _disposed;
    private long _bufferStart, _bufferEnd;
    private int _bufferIndex, _bufferLength;
    private bool _lastBuffer;
    private bool EndOfBuffer => _bufferIndex >= _bufferLength;
    
    public readonly Encoding Encoding;
    public readonly int CacheSize;
    public long Position => _bufferStart + _bufferIndex;
    public long Length => _stream.Length;
    public char? Previous;
    public char? Current;
    public bool EOF => Current == null || Position > Length || (EndOfBuffer && _lastBuffer);

    // ReSharper disable once UnusedParameter.Local C# why no duplicate constructors :(
    private Lexer(Stream stream, Encoding encoding, int cacheSize, bool _)
    {
        _stream = stream;
        if(!stream.CanSeek) throw new InvalidOperationException("Stream does not support seeking which is needed in the current lexing process.");
        Encoding = encoding;
        _encoder = Encoding.GetEncoder();
        CacheSize = cacheSize;
        _maxBufferReadLength = Encoding.GetMaxByteCount(CacheSize);
        _buffer = new char[CacheSize];
        _bufferStart = _stream.Position;
    }
    
    public Lexer(Stream stream, Encoding encoding, int cacheSize = 8) : this(stream, encoding, cacheSize, false)
    {
        ConsumeBuffer();
    }

    ~Lexer()
    {
        Dispose();
    }

    public void StepBackward()
    {
        if (_bufferIndex == 0 || (Previous == null && _bufferStart != 0))
        {
            if (_bufferStart == 0)
            {
                throw new Exception("The start of the stream has been reached. Can't step back further.");
            }
            Array.Copy(_buffer, 0, _buffer, 1, _buffer.Length - 1);
            _buffer[0] = PeekAt(--_bufferStart) ?? throw new Exception("This shouldn't happen.");
            Current = _buffer[0];
            Previous = _bufferIndex == 0 ? null : _buffer[_bufferIndex - 1];
            return;
        } 
        _bufferIndex--;
        Current = _buffer[_bufferIndex];
        Previous = _bufferIndex == 0 ? null : _buffer[_bufferIndex - 1];
    }
    
    public void StepForward()
    {
        _bufferIndex += 1;
        if (!EndOfBuffer)
        {
            Previous = Current;
            Current = _buffer[_bufferIndex];
            return;
        } 
        ConsumeBuffer();
    }

    public bool Take(char? target)
    {
        if (Current == target)
        {
            StepForward();
            return true;
        }

        return false;
    }

    public bool Give(char? target)
    {
        if (Current != target)
        {
            StepForward();
            return true;
        }

        return false;
    }

    public char? Consume()
    {
        var target = Current;
        StepForward();
        return target;
    }
    
    public char? ConsumeNot(char target)
    {
        var last = Consume();
        while (!EOF && last == target)
        {
            last = Consume();
        }

        return last;
    }

    private char? PeekAt(long index)
    {
        var oldPos = _stream.Position;
        try
        {
            lock (_stream)
            {
                _stream.Position = index;
                var maxByteCount = Encoding.GetMaxByteCount(1);
                Span<byte> buffer = stackalloc byte[maxByteCount];
                var bytesRead = _stream.Read(buffer);
                if (bytesRead == 0)
                {
                    return null;
                }
                
                Span<char> decodedChars = stackalloc char[Encoding.GetMaxCharCount(maxByteCount)];
                if( Encoding.GetChars(buffer, decodedChars ) < 1) return null;
                return decodedChars[0];
            }
        }
        finally
        {
            _stream.Seek(oldPos, SeekOrigin.Begin);
        }
    }

    public char? Peek()
    {
        if (_bufferIndex + 1 >= _bufferLength)
        {
            return _lastBuffer ? null : PeekAt(Position - 1);
        }
        
        return _buffer[_bufferIndex + 1];
    }

    public char? ConsumeCountNot(char target, out uint count)
    {
        char? last = null;
        for (count = 0; !EOF && (last = Consume()) != target; count++) { }
        return last;
    }

    public void SeekUntil(char target)
    {
        while (!EOF && Give(target))
        {
        }
    }
    
    public string ConsumeUntil(char target)
    {
        var result = new StringBuilder();
        while (!EOF && Give(target))
        {
            result.Append(Previous);
        }

        return result.ToString();
    }
    
    private void ConsumeBuffer()
    {
        lock (_stream)
        {
            Previous = Current;
            Span<byte> bBuffer = stackalloc byte[_maxBufferReadLength];
            var bytesRead = _stream.Read(bBuffer);
            _bufferIndex = 0;
            Span<char> chars = stackalloc char[CacheSize];

            _encoder.Convert(
                chars,
                bBuffer[..bytesRead],
                flush: false,
                out var bytesUsed,
                out var charsProduced,
                out _
            );
            chars[..charsProduced].CopyTo(_buffer);
            if (bytesUsed < bytesRead)
            {
                _stream.Position -= bytesRead - bytesUsed;
            }

            _bufferLength = charsProduced;
            
            _bufferStart += bytesUsed;
            Current = _bufferLength == 0 ? null : _buffer[_bufferIndex];
            _bufferEnd = _bufferStart + _bufferLength;
            _lastBuffer = _bufferLength != CacheSize;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {

            if (disposing)
            {
                _stream.Dispose();
            }
            

            _disposed = true;
        }
    }

}