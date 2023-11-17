using System.Collections.Concurrent;
using BiSharper.Rv.Param.Models.Value;

namespace BiSharper.Rv.Param.Models.Statement;

public class ParamContext : IParamContext
{
    public required IParamContext ParentContext { get; init; }
    public required string ContextName { get; init; }
    public required string? ConformsTo { get; init; }
    public required ConcurrentDictionary<ParamParMeta, IParamValue> Parameters { get; init; } = new();
    public required ConcurrentDictionary<string, ParamContext> Contexts { get; init; }
    public required ConcurrentBag<IParamStatement> Statements { get; init; } = new();
}