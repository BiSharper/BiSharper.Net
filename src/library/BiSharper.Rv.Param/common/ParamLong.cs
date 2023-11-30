namespace BiSharper.Rv.Param.Common;

[ParamValue(ParamValueType.Long)]
public readonly struct ParamLong(long value, ParamContext parent): IParamValue
{
    public long Value { get; } = value;
    public ParamContext ParentContext { get; } = parent;
    public object ValueUnwrapped => Value;

    public static implicit operator long(ParamLong self) => self.Value;
    
    public string ToText() => Value.ToString();
}