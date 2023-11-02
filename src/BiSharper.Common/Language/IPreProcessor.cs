namespace BiSharper.Common.Language;

public interface IPreProcessor
{
    void Process(ref Lexer lexer);
}