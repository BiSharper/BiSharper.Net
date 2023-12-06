using BiSharper.BisIO.Lex;

namespace BiSharper.BisIO.Parse;

public interface IProcessed
{
    void Process(ref Lexer lexer);
}
