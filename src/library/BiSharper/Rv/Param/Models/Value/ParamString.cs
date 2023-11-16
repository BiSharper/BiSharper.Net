namespace BiSharper.Rv.Param.Models.Value;

public readonly struct ParamString : IParamValue
{
    public required string Value { get; init; }
    public required IParamContext ParentContext { get; init; }

    public static implicit operator string(ParamString self) => self.Value;
    
    public string ToText() => Value;
}