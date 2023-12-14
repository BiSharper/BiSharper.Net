using System.Collections;

namespace BiSharper.Rv.Param.AST.Value;


public readonly struct ParamArray : IParamArray<IParamValue>
{
    public List<IParamValue> Values { get; }
    public ParamArray(IEnumerable<IParamValue> values) => Values = new List<IParamValue>(values);
    public IEnumerator<IParamValue> GetEnumerator() =>
        Values.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();
}

public readonly struct ParamArray<T> : IParamArray<T> where T : IParamValue
{
    public List<T> Values { get; }
    public ParamArray(IEnumerable<T> values) => Values = new List<T>(values);
    public IEnumerator<T> GetEnumerator() =>
        Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();
}