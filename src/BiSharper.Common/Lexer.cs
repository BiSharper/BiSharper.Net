namespace BiSharper.Common;

public class Lexer<T>
{
    private readonly T[] _contents;
    public int Position { get; set; }
    public int Length => _contents.Length;
    public T? Previous => _contents.ElementAtOrDefault(Position - 1);
    public T? Current => _contents.ElementAtOrDefault(Position);
    public T? Next => _contents.ElementAtOrDefault(Position + 1);

    public Lexer(T[] contents)
    {
        _contents = contents;
        Position = 0;
    }


    public void StepBack() => Position--;
    public void StepForward() => Position++;
    
    public bool IsEOF() => Position >= Length;
    public void Reset() => Position = 0;

    public bool Take(T target)
    {
        if (Current?.Equals(target) == true)
        {
            StepForward();
            return true;
        }

        return false;
    }

    public T? Consume()
    {
        var target = Current;
        StepForward();
        return target;
    }

    public void SeekUntil(T target)
    {
        while (!IsEOF() && Current?.Equals(target) == false)
        {
            StepForward();
        }
    }

    public (uint, T?) ConsumeNot(T target)
    {
        uint count = 0;
        for (; !IsEOF() && Consume()?.Equals(target) == true; count++) {}
        return (count, Current);
    }




}