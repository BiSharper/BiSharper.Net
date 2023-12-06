using BiSharper.Rv.Param.Common.AST.Abstraction;

namespace BiSharper.Rv.Param.Common.AST.Statement;

public readonly struct ParamDeleteStatement : IParamStatement
{
    public string ContextName { get; }
    public IParamElement Parent => (IParamElement) ParentContext;
    public ParamContext ParentContext { get; }

    public ParamDeleteStatement(string target, ParamContext parent)
    {
        ContextName = target;
        ParentContext = parent;
    }
}