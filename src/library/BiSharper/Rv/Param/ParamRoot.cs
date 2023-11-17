using System.Collections.Concurrent;
using BiSharper.Rv.Param.Models;
using BiSharper.Rv.Param.Models.Statement;
using BiSharper.Rv.Param.Models.Value;

namespace BiSharper.Rv.Param;

public partial struct ParamRoot : IParamContext
{
    public string ContextName { get; init; }
    public ConcurrentBag<IParamStatement> Statements { get; init; } = new();
    public ConcurrentDictionary<ParamParMeta, IParamValue> Parameters { get; init; } = new();
    public ConcurrentDictionary<string, ParamContext> Contexts { get; init; } = new();

    public ParamRoot(string name)
    {
        ContextName = name;
    }
    
    
    
}