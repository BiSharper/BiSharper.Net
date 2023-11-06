namespace BiSharper.Rv.Param.Models.Value;

public readonly struct ParamFloat : IParamValue
{
    public required float Value { get; init; }

    public static implicit operator ParamFloat(float value) => new ParamFloat { Value = value };
    
    public static implicit operator float(ParamFloat self) => self.Value;
    
    public string ToText() => Value.ToString("E");
}