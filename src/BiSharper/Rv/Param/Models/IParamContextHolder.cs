using System.Collections.Concurrent;
using BiSharper.Rv.Param.Models.Statement;

namespace BiSharper.Rv.Param.Models;

public interface IParamContextHolder
{
    public ConcurrentDictionary<string, IParamStatement> Statements { get; init; }
}