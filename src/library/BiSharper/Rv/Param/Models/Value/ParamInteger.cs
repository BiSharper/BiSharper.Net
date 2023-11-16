namespace BiSharper.Rv.Param.Models.Value;

public readonly struct ParamInteger : IParamValue
{
    public required int Value { get; init; }
    public required IParamContext ParentContext { get; init; }
    
    public static implicit operator int(ParamInteger self) => self.Value;
    
    public string ToText() => Value.ToString();
}