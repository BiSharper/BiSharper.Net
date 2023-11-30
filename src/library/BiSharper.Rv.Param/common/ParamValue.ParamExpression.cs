namespace BiSharper.Rv.Param.Common;

[ParamValue(ParamValueType.Expression)]
public readonly struct ParamExpression(string value, ParamContext parent) : IParamValue
{
    public string Value { get; } = value;
    public ParamContext ParentContext { get; } = parent;
    public object ValueUnwrapped => Value;

    public static implicit operator string(ParamExpression self) => self.Value;

    public string ToText() => Value;
}