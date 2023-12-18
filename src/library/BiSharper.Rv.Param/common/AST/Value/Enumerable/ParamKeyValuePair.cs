using System.Collections;

namespace BiSharper.Rv.Param.AST.Value.Enumerable;

// public readonly struct ParamKeyValuePair<TKey, TValue> : IParamArray<IParamValue>
//     where TKey : IParamValue
//     where TValue : IParamValue
// {
//     public TKey Key { get; }
//     public TValue Value { get; }
//
//     public ParamKeyValuePair(TKey key, TValue value)
//     {
//         Key = key;
//         Value = value;
//     }
//
//     public IEnumerator<IParamValue> GetEnumerator()
//     {
//         yield return Key;
//         yield return Value;
//     }
//
//     IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
// }