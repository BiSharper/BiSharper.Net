﻿using System.Collections;
using System.Collections.Concurrent;
using BiSharper.Rv.Param.Common.AST.Abstraction;
using BiSharper.Rv.Param.Common.AST.Statement;

namespace BiSharper.Rv.Param.Common.AST.Value;

public readonly struct ParamArray : IEnumerable<IParamValue>
{
    public ConcurrentBag<IParamValue> Values { get; }
    public IParamElement Parent { get; }

    public ParamArray(IEnumerable<IParamValue> values, IParamElement parent)
    {
        Values = new ConcurrentBag<IParamValue>(values);
        Parent = parent;
    }

    public static implicit operator ConcurrentBag<IParamValue>(ParamArray self) =>
        self.Values;

    public IEnumerator<IParamValue> GetEnumerator() =>
        Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();
}