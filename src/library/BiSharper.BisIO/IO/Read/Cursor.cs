using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace BiSharper.BisIO.IO.Read;

internal ref struct Cursor
{
    private readonly ReadOnlySequence<byte> _sequence;
    private readonly bool _isInputSequence, _isMultiSegment;
    private bool _isLastSegment;
    private ReadOnlySpan<byte> _segmentSpan;
    private ReadOnlyMemory<byte>? _nextSegmentMemory;
    private int _segmentIndex;
    private SequencePosition _currentSegment;
    private SequencePosition? _nextSegment;
    private bool IsLastSpan => !_isMultiSegment || _isLastSegment;
    private SequencePosition SequencePosition => _sequence.GetPosition(_segmentIndex, _currentSegment);
    public readonly long Length => _isMultiSegment ? _sequence.Length : _segmentSpan.Length;

    public bool HasNext
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _segmentIndex < _segmentSpan.Length || !IsLastSpan;
    }

    public int Position
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            if (!_isInputSequence) return _segmentIndex;
            Debug.Assert(_currentSegment.GetObject() != null);
            return _currentSegment.GetInteger() + _segmentIndex;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Cursor(ReadOnlySequence<byte>? sequence, ReadOnlySpan<byte> data)
    {

        _segmentIndex = 0;
        _sequence = sequence ?? default;
        _segmentSpan = data;
        if (sequence is null)
        {
            _isLastSegment = true;
            _isMultiSegment = false;
            _isInputSequence = false;
        }
        else
        {
            _isInputSequence = false;
            _isMultiSegment = sequence.Value.IsSingleSegment;
            if (_isMultiSegment) _isLastSegment = true;
        }
    }

    public Cursor(ReadOnlySpan<byte> data) : this(null, data)
    {
        _currentSegment = default;
        _nextSegment = null;
        _nextSegmentMemory = null;
    }

    public Cursor(ReadOnlySequence<byte> sequence) : this(sequence, sequence.FirstSpan)
    {
        _currentSegment = _sequence.Start;
        _isLastSegment = _isMultiSegment;
        TryGetNextSegment(out _nextSegment, out _nextSegmentMemory);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public byte? PeekNext()
    {
        if (_segmentIndex < _segmentSpan.Length - 1) return _segmentSpan[_segmentIndex + 1];
        return !_isLastSegment ? _nextSegmentMemory?.Span[0] : null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public byte? ContentAt(int? position = null) => (position ??= _segmentIndex) > Length ? null : _segmentSpan[(int)position];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IncrementCursor(int count = 1) =>
        HasNext && ((_segmentIndex += count) <= _segmentSpan.Length || (!_isLastSegment && GetNextSegment()));

    private bool GetNextSegment()
    {
        if (!_isMultiSegment || _nextSegment is null || _nextSegmentMemory is null) return false;
        _segmentIndex -= _segmentSpan.Length;
        _segmentSpan = _nextSegmentMemory.Value.Span;
        _currentSegment = _nextSegment.Value;
        _isLastSegment = TryGetNextSegment(out _nextSegment, out _nextSegmentMemory);
        return true;
    }


    private byte? SkipNext(byte? target, Func<byte, bool>? predicate, out int bytesSkipped, int count = 1)
    {
        Debug.Assert(target != null && predicate != null);
        if (count <= -1)
        {
            bytesSkipped = 0;
            while (HasNext)
            {
                var next = PeekNext();
                if (next is not { } n || (predicate?.Invoke(n) ?? n != target)) return next;

                IncrementCursor();
                bytesSkipped++;
            }
            return null;
        }

        bytesSkipped = Math.Min(count, _segmentSpan.Length - _segmentIndex);
        for (var i = 0; i < bytesSkipped; i++)
        {
            var next = PeekNext();
            if (next is not { } n || (predicate?.Invoke(n) ?? n != target)) return next;

            IncrementCursor();
        }
        return PeekNext();
    }

    public byte? SkipNext(byte target, out int bytesSkipped, int count = 1) =>
        SkipNext(target, null, out bytesSkipped, count);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public byte? SkipNext(Func<byte, bool> predicate, out int bytesSkipped, int count = 1) =>
        SkipNext(null, predicate, out bytesSkipped, count);


    private bool TryGetNextSegment(out SequencePosition? position, out ReadOnlyMemory<byte>? memory)
    {
        _nextSegment = _currentSegment;
        if (
            !_isMultiSegment
            || !NextSegmentDataRelativeTo(ref _nextSegment, out var segmentData)
        )
        {
            memory = null;
            position = _currentSegment;
            return false;
        }


        memory = segmentData;
        position = _nextSegment!.Value;
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool NextSegmentDataRelativeTo(ref SequencePosition? startPosition, out ReadOnlyMemory<byte> memory)
    {
        var pos = startPosition ??= _currentSegment;
        var result = _sequence.TryGet(ref pos, out memory, advance: true);
        startPosition = pos;
        return result;
    }
}