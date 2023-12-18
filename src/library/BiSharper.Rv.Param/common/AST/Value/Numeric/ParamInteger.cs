namespace BiSharper.Rv.Param.AST.Value.Numeric;

public interface IParamInteger : IParamValue
{
    public int Value { get; }
}

public readonly struct ParamInteger : IParamInteger
{
    public int Value { get; }
    public ParamInteger(int value) => Value = value;
    public static explicit operator ParamInteger(int self) => new(self);
    public static implicit operator int(ParamInteger self) => self.Value;
}