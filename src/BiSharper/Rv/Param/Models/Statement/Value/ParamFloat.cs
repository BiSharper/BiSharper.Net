namespace BiSharper.Rv.Param.Models.Statement.Value;

public readonly struct ParamFloat : IParamValue
{
    public required float Value { get; init; }
    public required IParamContextHolder ParentContextHolder { get; init; }
    
    public static implicit operator float(ParamFloat self) => self.Value;
    
    public string ToText() => Value.ToString("E");
}