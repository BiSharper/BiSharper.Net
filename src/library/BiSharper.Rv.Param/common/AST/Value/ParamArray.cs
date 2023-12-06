using System.Collections;
using System.Collections.Concurrent;
using BiSharper.Rv.Param.AST.Abstraction;

namespace BiSharper.Rv.Param.AST.Value;

public readonly struct ParamArray : IEnumerable<IParamValue>, IParamValue
{
    public ConcurrentBag<IParamValue> Values { get; }
    public ParamArray(IEnumerable<IParamValue> values) => Values = new ConcurrentBag<IParamValue>(values);
    public IEnumerator<IParamValue> GetEnumerator() =>
        Values.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();
}