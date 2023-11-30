namespace BiSharper.Rv.Param.Common;

[ParamValue(ParamValueType.Integer)]
public readonly struct ParamInteger(int value, ParamContext parent) : IParamValue
{
    public int Value { get; } = value;
    public ParamContext ParentContext { get; } = parent;

    public static implicit operator int(ParamInteger self) => self.Value;

    public object ValueUnwrapped => Value;
    public string ToText() => Value.ToString();
}