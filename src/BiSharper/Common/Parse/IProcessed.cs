using BiSharper.Common.Lex;

namespace BiSharper.Common.Parse;

public interface IProcessed
{
    void Process(ref Lexer lexer);
}

public delegate Lexer IncludeLocator(string path); 