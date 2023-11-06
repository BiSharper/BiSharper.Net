namespace BiSharper.Rv.Param.Models.Value;

public readonly struct ParamInteger : IParamValue
{
    public required int Value { get; init; }
    

    public static implicit operator ParamInteger(int value) => new ParamInteger { Value = value };
    
    public static implicit operator int(ParamInteger self) => self.Value;
    
    public string ToText() => Value.ToString();
}