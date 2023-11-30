namespace BiSharper.Rv.Param.Common;

[ParamValue(ParamValueType.Float)]
public readonly struct ParamFloat(float value, ParamContext parent) : IParamValue
{
    public float Value { get; } = value;
    public ParamContext ParentContext { get; } = parent;
    public object ValueUnwrapped => Value;

    public static implicit operator float(ParamFloat self) => self.Value;
    
    public string ToText() => Value.ToString("E");
}