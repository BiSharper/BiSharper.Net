using BiSharper.Rv.Param.AST.Abstraction;

namespace BiSharper.Rv.Param.AST.Value;

public interface IParamExpression : IParamString;

public readonly struct ParamExpression : IParamExpression
{
    public string Value { get; }
    public ParamExpression(string value) => Value = value;
}