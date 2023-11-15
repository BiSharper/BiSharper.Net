namespace BiSharper.Rv.Param.Models.Statement;

public readonly struct ParamExternalContext: IParamStatement
{
    public required string ContextName { get; init; }
    public required IParamContextHolder ParentContextHolder { get; init; }
}