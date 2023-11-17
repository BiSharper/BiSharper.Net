using BiSharper.Rv.Param.Models.Statement;

namespace BiSharper.Rv.Param.Models.Value;

[ParamValue(ParamValueType.Expression)]
public readonly struct ParamExpression : IParamValue
{
    public required string Value { get; init; }
    public required IParamContext ParentContext { get; init; }

    public static implicit operator string(ParamExpression self) => self.Value;

    public string ToText() => Value;
}