using System.Collections;
using BiSharper.Rv.Param.AST.Abstraction;

namespace BiSharper.Rv.Param.AST.Value.Enumerable;

public readonly struct ParamReadOnlyList<T> : IParamArray<T>, IReadOnlyList<T> where T : IParamValue
{
    private T[] Values { get; }

    public int Count => Values.Length;

    public ParamReadOnlyList(T[] values) => Values = values;

    public ParamReadOnlyList(IEnumerable<T> values) : this(values.ToArray())
    {
    }

    public ParamList<T> ToParamList() => new(Values);

    public static implicit operator T[](ParamReadOnlyList<T> self) => self.Values;

    public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)Values).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


    public bool Contains(T item) => Values.Contains(item);

    public void CopyTo(T[] array, int arrayIndex) => Values.CopyTo(array, arrayIndex);

    public T this[int index] => Values[index];
}