using System.Collections;
using System.Collections.Concurrent;

namespace BiSharper.Rv.Param.Models.Value;

[ParamValue(ParamValueType.Array)]
public readonly struct ParamArray : IParamArray
{
    public required ConcurrentBag<IParamValue> Values { get; init; } = new();
    public required IParamContext ParentContext { get; init; }

    public ParamArray()
    {
    }
    
    public static implicit operator ConcurrentBag<IParamValue>(ParamArray self) => self.Values;
    
    public IEnumerator<IParamValue> GetEnumerator() => Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


}