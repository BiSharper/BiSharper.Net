using System.Collections;
using System.Collections.Concurrent;

namespace BiSharper.Rv.Param.Common;

[ParamValue(ParamValueType.Array, ParamOperatorType.ArrayOperations)]
public readonly struct ParamArray(IEnumerable<IParamValue> values, ParamContext parent) : IEnumerable<IParamValue>
{

    public ConcurrentBag<IParamValue> Values { get; } = new (values);
    public ParamContext ParentContext { get; } = parent;

    public static implicit operator ConcurrentBag<IParamValue>(ParamArray self) => self.Values;
    
    public IEnumerator<IParamValue> GetEnumerator() => Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public string ToText() =>
        '{' + string.Join(", ", this.Select(s => s.ToText())) + '}';
}