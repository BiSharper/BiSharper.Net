namespace BiSharper.Rv.Param.Models.Value;

public readonly struct ParamLong: IParamValue
{
    public required long Value { get; init; }
    public required IParamContextHolder ParentContextHolder { get; init; }

    public static implicit operator long(ParamLong self) => self.Value;
    
    public string ToText() => Value.ToString();
}