using System.Collections.Concurrent;
using BiSharper.Rv.Param.Models;
using BiSharper.Rv.Param.Models.Statement;
using BiSharper.Rv.Param.Models.Statement.Value;

namespace BiSharper.Rv.Param;

public partial struct ParamRoot : IParamContextHolder
{
    public readonly string Name;
    
    public ConcurrentDictionary<string, IParamStatement> Statements { get; init; }

    public ParamRoot(string name)
    {
        Name = name;
    }
    
    
    
}