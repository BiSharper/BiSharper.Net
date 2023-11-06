using System.Collections.Concurrent;
using BiSharper.Rv.Param.Models.Statement;
using BiSharper.Rv.Param.Models.Value;

namespace BiSharper.Rv.Param.Models;

public class ParamContext : IParamContextHolder
{
    public required IParamContextHolder Parent { get; init; }
    public required string? ConformsTo { get; init; }
    public required ConcurrentDictionary<string, IParamValue> Parameters { get; init; } = new();
    public required ConcurrentDictionary<string, ParamContext> Contexts { get; init; } = new();
    public required ConcurrentDictionary<string, IParamStatement> Statements { get; init; } = new();
  
    public ParamContext? ParentContext => Parent as ParamContext;

    public ParamContext(IParamContextHolder parent, string? super)
    {
        ConformsTo = super;
        Parent = parent;
    }
}