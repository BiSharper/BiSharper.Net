using BiSharper.Rv.Param.AST.Abstraction;

namespace BiSharper.Rv.Param.AST.Statement;

public interface IParamClass : IParamContext, IParamContextualStatement
{
    public string? ConformsTo { get; }
}

public sealed class ParamClass : ParamContext, IParamClass
{
    public IParamContext ParentContext { get; }
    public string? ConformsTo { get; set; }


    public ParamClass(string name, IParamContext parent, string? super = null) : base(name)
    {
        ParentContext = parent;
        ConformsTo = super;
    }

}