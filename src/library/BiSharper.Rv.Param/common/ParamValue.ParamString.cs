namespace BiSharper.Rv.Param.Common;

[ParamValue(ParamValueType.String)]
public readonly struct ParamString(string value, ParamContext parent) : IParamValue
{
    public string Value { get; } = value;
    public ParamContext ParentContext { get; } = parent;
    public object ValueUnwrapped => Value;

    public static implicit operator string(ParamString self) => self.Value;
    
    public string ToText() => Value;
}