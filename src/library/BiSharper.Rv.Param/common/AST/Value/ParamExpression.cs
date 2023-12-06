using BiSharper.Rv.Param.Common.AST.Abstraction;

namespace BiSharper.Rv.Param.Common.AST.Value;

public readonly struct ParamExpression : IParamValue
{
    public string Value { get; }
    public ParamExpression(string value) => Value = value;
}