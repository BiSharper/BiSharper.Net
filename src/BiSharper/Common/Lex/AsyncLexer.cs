using System.Text;

namespace BiSharper.Common.Lex;

public sealed partial class Lexer : IAsyncDisposable
{
    public static async ValueTask<Lexer> CreateAsyncLexer(Stream stream, Encoding encoding, int cacheSize = 8,
        CancellationToken cancellationToken = default)
    {
        var lexer = new Lexer(stream, encoding, cacheSize, false);
        await lexer.ConsumeBufferAsync(cancellationToken);
        return lexer;
    }
    
    public async Task StepForwardAsync(CancellationToken cancellationToken = default)
    {
        _bufferIndex += 1;
        if (!EndOfBuffer)
        {
            Previous = Current;
            Current = _buffer[_bufferIndex];
            return;
        } 
        await ConsumeBufferAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task StepBackwardAsync(CancellationToken cancellationToken = default)
    {
        if (_bufferIndex == 0 || (Previous == null && _bufferStart != 0))
        {
            if (_bufferStart == 0)
            {
                throw new Exception("The start of the stream has been reached. Can't step back further.");
            }
            Array.Copy(_buffer, 0, _buffer, 1, _buffer.Length - 1);
            _buffer[0] = await PeekAtAsync(--_bufferStart, cancellationToken).ConfigureAwait(false) ?? throw new Exception("This shouldn't happen.");
            Current = _buffer[0];
            Previous = null;
            return;
        } 
        _bufferIndex--;
        Current = _buffer[_bufferIndex];
        Previous = _bufferIndex == 0 ? null : _buffer[_bufferIndex - 1];
    }
    
    public async ValueTask<bool> TakeAsync(char? target, CancellationToken cancellationToken = default)
    {
        if (Current == target)
        {
            await StepForwardAsync(cancellationToken).ConfigureAwait(false);
            return true;
        }

        return false;
    }
    
    public async ValueTask<bool> GiveAsync(char? target, CancellationToken cancellationToken = default)
    {
        if (Current != target)
        {
            await StepForwardAsync(cancellationToken).ConfigureAwait(false);
            return true;
        }

        return false;
    }
    
    public async ValueTask<char?> PeekAtAsync(long index, CancellationToken cancellationToken = default)
    {
        var oldPos = _stream.Position;
        try
        {
            _stream.Position = index;
            var maxByteCount = Encoding.GetMaxByteCount(1);
            Memory<byte> buffer = new byte[maxByteCount];
            var bytesRead = await _stream.ReadAsync(buffer, cancellationToken).ConfigureAwait(false);
            if (bytesRead == 0) return null;
            
            Memory<char> decodedChars = new char[Encoding.GetMaxCharCount(maxByteCount)];
            if(Encoding.GetChars(buffer.Span, decodedChars.Span) < 1) return null;
            return decodedChars.Span[0];
        }
        finally
        {
            _stream.Seek(oldPos, SeekOrigin.Begin);
        }
    }
    
    public async ValueTask<char?> ConsumeAsync(CancellationToken cancellationToken = default)
    {
        var target = Current;
        await StepForwardAsync(cancellationToken).ConfigureAwait(false);
        return target;
    }
    
    public async ValueTask<char?> ConsumeNotAsync(char target, CancellationToken cancellationToken = default)
    {
        var last = await ConsumeAsync(cancellationToken).ConfigureAwait(false);
        while (!EOF && last == target)
        {
            last = await ConsumeAsync(cancellationToken).ConfigureAwait(false);
        }

        return last;
    }
    
    private async ValueTask ConsumeBufferAsync(CancellationToken cancellationToken = default)
    {
         Previous = Current;
         Memory<byte> bBuffer = new byte[_maxBufferReadLength];
         var bytesRead = await _stream.ReadAsync(bBuffer, cancellationToken)
             .ConfigureAwait(false);
         _bufferStart += CacheSize;
         _bufferIndex = 0;
         Memory<char> chars = new char[CacheSize];

         _encoder.Convert(
             chars.Span,
             bBuffer.Span[..bytesRead],
             flush: false,
             out var bytesUsed,
             out var charsProduced,
             out _
         );
         chars[..charsProduced].CopyTo(_buffer);
         if (bytesUsed < bytesRead) _stream.Position -= bytesRead - bytesUsed;

        _bufferLength = charsProduced;
        Current = _bufferLength == 0 ? null : _buffer[_bufferIndex];
        _bufferEnd = _bufferStart + _bufferLength;
        _lastBuffer = _bufferLength != CacheSize && _bufferEnd > Length;
    }
    
    
    
    public async ValueTask DisposeAsync()
    {
        await DisposeAsync(true);
        GC.SuppressFinalize(this);
    }
    
    
    private async ValueTask DisposeAsync(bool disposing)
    {
        if (!_disposed)
        {

            if (disposing)
            {
                await _stream.DisposeAsync();
            }
            

            _disposed = true;
        }
    }
}