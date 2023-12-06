using BiSharper.Rv.Param.Common.AST.Abstraction;

namespace BiSharper.Rv.Param.Common.AST.Statement;

public readonly struct ParamExternalContext : IParamStatement
{
    public string ContextName { get; }
    public IParamElement Parent => (IParamElement) ParentContext;
    public ParamContext ParentContext { get; }

    public ParamExternalContext(string name, ParamContext parent)
    {
        ContextName = name;
        ParentContext = parent;
    }
}