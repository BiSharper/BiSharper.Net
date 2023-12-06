using BiSharper.Rv.Param.Common.AST.Abstraction;
using BiSharper.Rv.Param.Common.AST.Statement;

namespace BiSharper.Rv.Param.Common.AST.Value;

public readonly struct ParamInteger : IParamValue
{
    public int Value { get; }
    public ParamInteger(int value) => Value = value;
    public static explicit operator ParamInteger(int self) => new(self);
    public static implicit operator int(ParamInteger self) => self.Value;
}