﻿using BiSharper.Rv.Param.Common.AST.Abstraction;

namespace BiSharper.Rv.Param.Common.AST.Value;

public readonly struct ParamString : IParamValue
{
    public string Value { get; }
    public ParamString(string value) => Value = value;
    public static explicit operator ParamString(string self) => new(self);
    public static implicit operator string(ParamString self) => self.Value;
}