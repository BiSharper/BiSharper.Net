namespace BiSharper.Rv.Param.Models.Value;

public readonly struct ParamExpression : IParamValue
{
    public required string Value { get; init; }
    public required IParamContextHolder ParentContextHolder { get; init; }

    public static implicit operator string(ParamExpression self) => self.Value;

    public string ToText() => Value;
}