using BiSharper.Rv.Param.AST.Abstraction;
using BiSharper.Rv.Param.AST.Value;

namespace BiSharper.Rv.Param.AST.Statement;

public sealed class ParamParameter : IParamContextualStatement
{
    public string Name { get; }
    public IParamValue Value { get; set; }
    public IParamContext ParentContext { get; }

    public ParamParameter(string name, IParamValue value, IParamContext parent)
    {
        Name = name;
        Value = value;
        ParentContext = parent;
    }
}