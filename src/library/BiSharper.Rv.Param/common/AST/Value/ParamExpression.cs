using BiSharper.Rv.Param.AST.Abstraction;

namespace BiSharper.Rv.Param.AST.Value;

public readonly struct ParamExpression : IParamValue
{
    public string Value { get; }
    public ParamExpression(string value) => Value = value;
}