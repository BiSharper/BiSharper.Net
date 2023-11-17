using System.Collections.Concurrent;
using BiSharper.Rv.Param.Models.Statement;
using BiSharper.Rv.Param.Models.Value;

namespace BiSharper.Rv.Param.Models;

public interface IParamContext
{
    public string ContextName { get; init; }
    public ConcurrentBag<IParamStatement> Statements { get; init; }
    public ConcurrentDictionary<ParamParMeta, IParamValue> Parameters { get; init; }
    public ConcurrentDictionary<string, ParamContext> Contexts { get; init; }
}

