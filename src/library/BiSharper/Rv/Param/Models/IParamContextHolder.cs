using System.Collections.Concurrent;
using BiSharper.Rv.Param.Models.Statement;
using BiSharper.Rv.Param.Models.Value;

namespace BiSharper.Rv.Param.Models;

public interface IParamContextHolder
{
    public ConcurrentBag<IParamStatement> Statements { get; init; }
    public ConcurrentDictionary<string, IParamValue> Parameters { get; init; }
    public ConcurrentDictionary<string, ParamContext> Contexts { get; init; }


    
}