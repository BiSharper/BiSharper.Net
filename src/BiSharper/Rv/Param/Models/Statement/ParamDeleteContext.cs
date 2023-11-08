namespace BiSharper.Rv.Param.Models.Statement;

public readonly struct ParamDeleteContext: IParamStatement
{
    public required string ContextName { get; init; }
    public required IParamContextHolder ParentContextHolder { get; init; }
}