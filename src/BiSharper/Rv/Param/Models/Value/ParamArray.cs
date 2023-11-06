using System.Collections;
using System.Collections.Concurrent;

namespace BiSharper.Rv.Param.Models.Value;

public readonly struct ParamArray : IParamArray
{
    public required ConcurrentBag<IParamValue> Values { get; init; } = new();
    
    public ParamArray()
    {
    }

    public static implicit operator ParamArray(ConcurrentBag<IParamValue> value) 
        => new ParamArray { Values = value };
    
    public static implicit operator ConcurrentBag<IParamValue>(ParamArray self) => self.Values;
    
    public IEnumerator<IParamValue> GetEnumerator() => Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    

}