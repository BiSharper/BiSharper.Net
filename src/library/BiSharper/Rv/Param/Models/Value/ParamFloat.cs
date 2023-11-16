namespace BiSharper.Rv.Param.Models.Value;

public readonly struct ParamFloat : IParamValue
{
    public required float Value { get; init; }
    public required IParamContext ParentContext { get; init; }
    
    public static implicit operator float(ParamFloat self) => self.Value;
    
    public string ToText() => Value.ToString("E");
}