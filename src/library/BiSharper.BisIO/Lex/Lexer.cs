using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace BiSharper.BisIO.Lex;

public ref struct Lexer
{
    private readonly ReadOnlySequence<byte> _sequence;
    private readonly bool _isInputSequence, _isMultiSegment;
    public readonly long Length => _isInputSequence ? _sequence.Length : _segmentSpan.Length;
    private ReadOnlySpan<byte> _segmentSpan;
    private ReadOnlyMemory<byte>? _nextSegmentMemory;
    private int SegmentIndex { get; set; }
    private SequencePosition _currentSegment;
    private SequencePosition? _nextSegment;
    private bool IsLastSegment { get; set; }

    private readonly bool IsLastSpan => !_isMultiSegment || IsLastSegment;

    private readonly SequencePosition SequencePosition => _sequence.GetPosition(SegmentIndex, _currentSegment);

    private readonly bool HasNext {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => SegmentIndex < _segmentSpan.Length || !IsLastSpan;
    }

    public readonly int Position
    {
        get
        {
            if (!_isInputSequence) return SegmentIndex;
            Debug.Assert(_currentSegment.GetObject() != null);
            return _currentSegment.GetInteger() + SegmentIndex;
        }
    }

    private Lexer(ReadOnlySequence<byte>? sequence, ReadOnlySpan<byte> data)
    {
        SegmentIndex = 0;
        if(sequence is null)
        {
            IsLastSegment = true;
            _isMultiSegment = true;
            _isInputSequence = false;
        }
        else
        {
            _isInputSequence = false;
            _isMultiSegment = sequence.Value.IsSingleSegment;
            if (_isMultiSegment) IsLastSegment = true;
        }
        _sequence = sequence ?? default;
        _segmentSpan = data;
    }

    public Lexer(ReadOnlySpan<byte> data) : this(null, data)
    {
        _currentSegment = default;
        _nextSegment = null;
        _nextSegmentMemory = null;
    }

    public Lexer(ReadOnlySequence<byte> sequence) : this(sequence, sequence.FirstSpan)
    {
        _currentSegment = _sequence.Start;
        IsLastSegment = _isMultiSegment;
        TryGetNextSegment(out _nextSegment, out _nextSegmentMemory);
    }

    public byte? ConsumeNext()
    {
        if(!HasNext) return null;
        SegmentIndex++;

        if (SegmentIndex >= _segmentSpan.Length && !IsLastSegment) NextSegment();

        return _segmentSpan[SegmentIndex];
    }

    public byte? PeekNext()
    {
        if (SegmentIndex < _segmentSpan.Length - 1) return _segmentSpan[SegmentIndex + 1];
        return !IsLastSegment ? _nextSegmentMemory?.Span[0] : null;
    }

    private bool NextSegment()
    {
        if (!_isMultiSegment || _nextSegment is null || _nextSegmentMemory is null) return false;
        _segmentSpan = _nextSegmentMemory.Value.Span;
        SegmentIndex = 0;
        _currentSegment = _nextSegment.Value;
        IsLastSegment = TryGetNextSegment(out _nextSegment, out _nextSegmentMemory);
        return true;
    }

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