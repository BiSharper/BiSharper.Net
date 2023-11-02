namespace BiSharper.Common.Language;

public interface IBisParsable
{
    public void Parse(Lexer lexer);
}

public interface IBisParsable<in TPreProcessor>: IBisParsable where TPreProcessor: IPreProcessor
{
    public void Parse(Lexer lexer, TPreProcessor processor)
    {
        processor.Process(ref lexer);
        Parse(lexer);
    }

}