using BiSharper.Common.Lex;

namespace BiSharper.Common.Parse;

public interface IParsed
{
    public abstract void Parse(Lexer lexer);
}

public interface IParsed<in TPreProcessor>: IParsed where TPreProcessor: IProcessed
{
    

}

public static class ParsedExtensions
{
    public static void Parse<TPreProcessor>(this IParsed<TPreProcessor> parser, Lexer lexer, TPreProcessor processor) where TPreProcessor : IProcessed
    {
        processor.Process(ref lexer);
        parser.Parse(lexer);
    }
}