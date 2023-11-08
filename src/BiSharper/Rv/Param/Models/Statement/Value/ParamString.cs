﻿namespace BiSharper.Rv.Param.Models.Statement.Value;

public readonly struct ParamString : IParamValue
{
    public required string Value { get; init; }
    public required IParamContextHolder ParentContextHolder { get; init; }

    public static implicit operator string(ParamString self) => self.Value;
    
    public string ToText() => Value;
}