using System.Collections.Concurrent;
using BiSharper.Rv.Param.Models.Statement;
using BiSharper.Rv.Param.Models.Value;

namespace BiSharper.Rv.Param.Models;

public interface IParamContextHolder
{
    public ConcurrentDictionary<string, IParamValue> Parameters { get; init; }
    public ConcurrentDictionary<string, ParamContext> Contexts { get; init; }
    public ConcurrentDictionary<string, IParamStatement> Statements { get; init; }
}