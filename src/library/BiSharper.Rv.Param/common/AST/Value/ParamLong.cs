﻿using BiSharper.Rv.Param.Common.AST.Abstraction;
using BiSharper.Rv.Param.Common.AST.Statement;

namespace BiSharper.Rv.Param.Common.AST.Value;

public readonly struct ParamLong : IParamValue
{
    public long Value { get; }
    public ParamLong(long value) => Value = value;
    public static explicit operator ParamLong(long self) => new(self);
    public static implicit operator long(ParamLong self) => self.Value;
}