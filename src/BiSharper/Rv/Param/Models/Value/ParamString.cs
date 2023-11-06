namespace BiSharper.Rv.Param.Models.Value;

public readonly struct ParamString : IParamValue
{
    public required string Value { get; init; }

    public static implicit operator ParamString(string value) => new ParamString { Value = value };
    
    public static implicit operator string(ParamString self) => self.Value;
    
    public string ToText() => Value;
}