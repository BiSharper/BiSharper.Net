namespace BiSharper.Rv.Param.Models.Statement;

public readonly struct ParamDeleteStatement: IParamStatement
{
    public required string ContextName { get; init; }
}