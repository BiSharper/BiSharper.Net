namespace BiSharper.Common.IO;

public class Lexer
{
    private readonly char[] _contents;
    public int Position { get; set; }
    public int Length => _contents.Length;
    public char? Previous => _contents.ElementAtOrDefault(Position - 1);
    public char? Current => _contents.ElementAtOrDefault(Position);
    public char? Next => _contents.ElementAtOrDefault(Position + 1);

    public Lexer(char[] contents)
    {
        _contents = contents;
        Position = 0;
    }


    public void StepBack() => Position--;
    public void StepForward() => Position++;
    
    public bool IsEOF() => Position >= Length;
    public void Reset() => Position = 0;

    public bool Take(char target)
    {
        if (Current == target)
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

    public void SeekUntil(char target)
    {
        while (!IsEOF() && Current != target)
        {
            StepForward();
        }
    }

    public char[] ConsumeUntil(char target)
    {
        var start = Position;
        SeekUntil(target);
        return _contents[start..Position];
    }
    public char? ConsumeCountNot(char target, out uint count)
    {
        char? last = null;
        for (count = 0; !IsEOF() && (last = Consume()) == target; count++) {}
        return last;
    }

    public char? ConsumeNot(char target)
    {
        var last = Consume();
        while (!IsEOF() && last == target)
        {
            last = Consume();
        }

        return last;
    }
}