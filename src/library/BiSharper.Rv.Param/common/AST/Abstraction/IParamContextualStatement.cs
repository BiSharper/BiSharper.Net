using BiSharper.Rv.Param.AST.Statement;

namespace BiSharper.Rv.Param.AST.Abstraction;

public interface IParamContextualStatement : IParamStatement
{
    public IParamContext ParentContext { get; }
}