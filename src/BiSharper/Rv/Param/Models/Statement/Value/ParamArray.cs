using System.Collections;
using System.Collections.Concurrent;

namespace BiSharper.Rv.Param.Models.Statement.Value;

public readonly struct ParamArray : IParamArray
{
    public required ConcurrentBag<IParamValue> Values { get; init; } = new();
    public required IParamContextHolder ParentContextHolder { get; init; }

    public ParamArray()
    {
    }
    
    public static implicit operator ConcurrentBag<IParamValue>(ParamArray self) => self.Values;
    
    public IEnumerator<IParamValue> GetEnumerator() => Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


}