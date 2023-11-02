using BiSharper.Common.Language;
using BiSharper.RvProc;

namespace BiSharper.ParamFile;

public class Param : IBisParsable<RvProcessorContext>
{
    public void Parse(Lexer lexer) => ParamParser.Parse(lexer, this);
}