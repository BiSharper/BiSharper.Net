using BiSharper.Rv.Param.Common.AST.Abstraction;

namespace BiSharper.Rv.Param.Common.AST.Statement;

public sealed class ParamParameter : IParamStatement
{
    public string Name { get; }
    public IParamValue Value { get; set; }
    public IParamElement Parent => (IParamElement) ParentContext;
    public ParamContext ParentContext { get; }

    public ParamParameter(string name, IParamValue value, ParamContext parent)
    {
        Name = name;
        Value = value;
        ParentContext = parent;
    }
}