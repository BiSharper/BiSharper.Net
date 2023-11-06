using System.Collections.Concurrent;
using BiSharper.Rv.Param.Models;
using BiSharper.Rv.Param.Models.Statement;
using BiSharper.Rv.Param.Models.Value;

namespace BiSharper.Rv.Param;

public partial struct Param : IParamContextHolder
{
    public readonly string Name;
    
    public ConcurrentDictionary<string, IParamValue> Parameters { get; init; }
    public ConcurrentDictionary<string, ParamContext> Contexts { get; init; }
    public ConcurrentDictionary<string, IParamStatement> Statements { get; init; }

    public Param(string name)
    {
        Name = name;
    }
    
}