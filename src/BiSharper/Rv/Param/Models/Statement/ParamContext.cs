using System.Collections.Concurrent;
using BiSharper.Rv.Param.Models.Statement.Value;

namespace BiSharper.Rv.Param.Models.Statement;

public class ParamContext : IParamStatement, IParamContextHolder
{
    public required IParamContextHolder ParentContextHolder { get; init; }
    public required string? ConformsTo { get; init; }
    public required ConcurrentDictionary<string, IParamValue> Parameters { get; init; } = new();
    public required ConcurrentDictionary<string, IParamStatement> Statements { get; init; } = new();
  

    public ParamContext(IParamContextHolder parent, string? super)
    {
        ConformsTo = super;
        ParentContextHolder = parent;
    }

}