using System.Collections.Concurrent;
using BiSharper.Rv.Param.Models.Value;

namespace BiSharper.Rv.Param.Models;

public class ParamContext : IParamContextHolder
{
    public IParamContextHolder Parent { get; init; }
    public ParamContext? ParentContext => Parent as ParamContext;
    public string? ConformsTo { get; init; }
    public ConcurrentDictionary<string, IParamValue> Parameters { get; init; } = new();
    public ConcurrentDictionary<string, ParamContext> Contexts { get; init; } = new();

    public ParamContext(IParamContextHolder parent, string? super)
    {
        ConformsTo = super;
        Parent = parent;
    }
}