using BiSharper.Rv.Param.AST.Abstraction;

namespace BiSharper.Rv.Param.AST.Value;

public readonly struct ParamInteger : IParamValue
{
    public int Value { get; }
    public ParamInteger(int value) => Value = value;
    public static explicit operator ParamInteger(int self) => new(self);
    public static implicit operator int(ParamInteger self) => self.Value;
}