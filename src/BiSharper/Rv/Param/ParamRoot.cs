using System.Collections.Concurrent;
using BiSharper.Rv.Param.Models;
using BiSharper.Rv.Param.Models.Statement;
using BiSharper.Rv.Param.Models.Value;

namespace BiSharper.Rv.Param;

public partial struct ParamRoot : IParamContextHolder
{
    public readonly string Name;

    public ConcurrentBag<IParamStatement> Statements { get; init; } = new();
    public ConcurrentDictionary<string, IParamValue> Parameters { get; init; } = new();
    public ConcurrentDictionary<string, ParamContext> Contexts { get; init; } = new();

    public ParamRoot(string name)
    {
        Name = name;
    }
    
    
    
}