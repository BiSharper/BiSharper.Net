using BiSharper.Common.Lex;

namespace BiSharper.Common.Parse;

public interface IParsed
{
    public void Parse(Lexer lexer);
}

public interface IParsed<in TPreProcessor>: IParsed where TPreProcessor: IProcessed
{
    public void Parse(Lexer lexer, TPreProcessor processor)
    {
        processor.Process(ref lexer);
        Parse(lexer);
    }

}