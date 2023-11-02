namespace BiSharper.Common.Language;

public interface IPreProcessor
{
    void PreProcess(ref Lexer lexer);
}