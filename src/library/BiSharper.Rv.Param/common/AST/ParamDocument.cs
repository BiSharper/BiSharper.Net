using BiSharper.Rv.Param.AST.Abstraction;
using BiSharper.Rv.Param.AST.Statement;

namespace BiSharper.Rv.Param.AST;

public interface IParamDocument : IParamContext;

public sealed class ParamDocument : ParamContext, IParamDocument
{
    public ParamDocument(string name) : base(name)
    {
    }
}