using System.Buffers;
using System.Runtime.CompilerServices;

namespace BiSharper.BisIO.IO.Read;

public ref struct TraversableAnalyser
{
    private Cursor _cursor;
    public int Position => _cursor.Position;
    public long Length => _cursor.Length;
    public byte? CurrentByte { get; private set; }
    public byte? PreviousByte { get; private set; }
    public bool HasNext => _cursor.HasNext;

    internal TraversableAnalyser(Cursor cursor) => _cursor = cursor;

    public TraversableAnalyser(ReadOnlySpan<byte> data) : this(new Cursor(data))
    {
    }

    public TraversableAnalyser(ReadOnlySequence<byte> data) : this(new Cursor(data))
    {
    }

    public byte? ConsumeNext()
    {
        _cursor.IncrementCursor();
        SynchronizeWithCursor();
        return CurrentByte;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private byte? SynchronizeWithCursor() =>
        FastSynchronizeWithCursor(_cursor.ContentAt());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private byte? FastSynchronizeWithCursor(byte? newCurrent)
    {
        PreviousByte = CurrentByte;
        return CurrentByte = newCurrent;
    }

    public bool TakeNext(byte target, out int bytesSkipped, int count = 1)
    {
        SkipNext(target, out bytesSkipped, count);
        return bytesSkipped >= 1;
    }

    public bool TakeNext(Func<byte, bool> predicate, out int bytesSkipped, int count = 1)
    {
        SkipNext(predicate, out bytesSkipped, count);
        return bytesSkipped >= 1;
    }

    public bool InclusiveTakeNext(byte target, out int bytesSkipped, int count = 1)
    {
        PreviousByte = CurrentByte;
        return TakeNext(target, out bytesSkipped, count);
    }

    public bool InclusiveTakeNext(Func<byte, bool> predicate, out int bytesSkipped, int count = 1)
    {
        PreviousByte = CurrentByte;
        return TakeNext(predicate, out bytesSkipped, count);
    }

    public byte? SkipNext(int count = 1)
    {
        if (count <= 0)
            throw new ArgumentOutOfRangeException(nameof(count), "Cannot pass negative number to Lexer::SkipNext");
        _cursor.IncrementCursor(count);
        return CurrentByte;
    }

    public byte? InclusiveSkipNext(byte target, out int bytesSkipped, int count = 1)
    {
        PreviousByte = CurrentByte;
        return SkipNext(target, out bytesSkipped, count);
    }

    public byte? InclusiveSkipNext(Func<byte, bool> predicate, out int bytesSkipped, int count = 1)
    {
        PreviousByte = CurrentByte;
        return SkipNext(predicate, out bytesSkipped, count);
    }

    public byte? SkipNext(byte target, out int bytesSkipped, int count = 1) =>
        _cursor.SkipNext(target, out bytesSkipped, count);

    public byte? SkipNext(Func<byte, bool> predicate, out int bytesSkipped, int count = 1) =>
        _cursor.SkipNext(predicate, out bytesSkipped, count);

    public byte? SkipNextWhile(byte target, out int bytesSkipped) =>
        SkipNext(target, out bytesSkipped, -1);

    public byte? SkipNextWhile(Func<byte, bool> predicate, out int bytesSkipped) =>
        SkipNext(predicate, out bytesSkipped, -1);

    public byte? PeekNext() => _cursor.PeekNext();
}