using System.Collections;

namespace BiSharper.Rv.Param.AST.Value.Enumerable;

public readonly struct ParamList<T> : IParamArray<T>, IList<T> where T : IParamValue
{
    private List<T> Values { get; }

    public int Count => Values.Count;
    public bool IsReadOnly => false;

    public ParamList(List<T> values) => Values = values;

    public ParamList(IEnumerable<T> values) : this(values.ToList())
    {
    }

    public ParamList(T[] values) : this(values.ToList())
    {
    }

    public ParamList(Span<T> values) : this(values.ToArray())
    {
    }
    public static implicit operator List<T>(ParamList<T> self) => self.Values;

    public IEnumerator<T> GetEnumerator() => Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Add(T item) => Values.Add(item);

    public void Clear() => Values.Clear();

    public bool Contains(T item) => Values.Contains(item);

    public void CopyTo(T[] array, int arrayIndex) => Values.CopyTo(array, arrayIndex);

    public bool Remove(T item) => Values.Remove(item);

    public int IndexOf(T item) => Values.IndexOf(item);

    public void Insert(int index, T item) => Values.Insert(index, item);

    public void RemoveAt(int index) => Values.RemoveAt(index);

    public T this[int index]
    {
        get => Values[index];
        set => Values[index] = value;
    }
}
