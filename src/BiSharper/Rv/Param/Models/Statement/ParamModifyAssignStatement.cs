using BiSharper.Rv.Param.Models.Value;

namespace BiSharper.Rv.Param.Models.Statement;

public readonly struct ParamModifyAssignStatement: IParamStatement
{
    public required bool Negated { get; init; }
    public required IParamArray Value { get; init; }
}