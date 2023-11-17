using BiSharper.Rv.Param.Models.Statement;

namespace BiSharper.Rv.Param.Models.Value;

[ParamValue(ParamValueType.Float)]
public readonly struct ParamFloat : IParamValue
{
    public required float Value { get; init; }
    public required IParamContext ParentContext { get; init; }
    
    public static implicit operator float(ParamFloat self) => self.Value;
    
    public string ToText() => Value.ToString("E");
}