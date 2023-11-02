namespace BiSharper.Common.Language;

public interface IBisParseable<out TAST>
{
    public TAST Parse(Lexer lexer);
    
}

public interface IBisParsable<T, out TAST, in TPreProcessor>: IBisParseable<TAST> where TPreProcessor: IPreProcessor
{
    public TAST Parse(Lexer lexer, TPreProcessor processor)
    {
        processor.PreProcess(ref lexer);
        return Parse(lexer);
    }

}