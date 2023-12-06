using BiSharper.Rv.Param.Common.AST.Abstraction;

namespace BiSharper.Rv.Param.Common.AST.Statement;

public sealed class ParamClass : ParamContext, IParamContextualStatement
{
    public IParamElement Parent => (IParamElement) ParentContext;
    public ParamContext ParentContext { get; }
    public string? ConformsTo { get; init; }

    public ParamClass(string name, ParamContext parent, string? super = null) : base(name)
    {
        ParentContext = parent;
        ConformsTo = super;
    }
}