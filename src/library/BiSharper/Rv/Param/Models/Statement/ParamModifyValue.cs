using BiSharper.Rv.Param.Models.Value;

namespace BiSharper.Rv.Param.Models.Statement;

public readonly struct ParamModifyValue: IParamStatement
{
    public required bool Negated { get; init; }
    public required string PropertyName { get; init; }
    public required IParamArray Value { get; init; }
    public required IParamContext ParentContext { get; init; }
}