namespace BiSharper.Rv.Param.Models.Value;

public readonly struct ParamLong: IParamValue
{
    public required long Value { get; init; }
    
    public static implicit operator ParamLong(long value) => new ParamLong { Value = value };
    
    public static implicit operator long(ParamLong self) => self.Value;
    
    public string ToText() => Value.ToString();
}