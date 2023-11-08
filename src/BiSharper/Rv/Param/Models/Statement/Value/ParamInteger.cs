namespace BiSharper.Rv.Param.Models.Statement.Value;

public readonly struct ParamInteger : IParamValue
{
    public required int Value { get; init; }
    public required IParamContextHolder ParentContextHolder { get; init; }
    
    public static implicit operator int(ParamInteger self) => self.Value;
    
    public string ToText() => Value.ToString();
}