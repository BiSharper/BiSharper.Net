using BiSharper.Rv.Param.AST.Abstraction;

namespace BiSharper.Rv.Param.AST.Value;

public readonly struct ParamDouble : IParamValue
{
    public long Value { get; }
    public ParamDouble(long value) => Value = value;
    public static explicit operator ParamDouble(long self) => new(self);
    public static implicit operator long(ParamDouble self) => self.Value;
}