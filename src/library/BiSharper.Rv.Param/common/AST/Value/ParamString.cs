namespace BiSharper.Rv.Param.AST.Value;

public interface IParamString : IParamValue
{
    public string Value { get; }
}

public readonly struct ParamString : IParamString
{
    public string Value { get; }
    public ParamString(string value) => Value = value;
    public static explicit operator ParamString(string self) => new(self);
    public static implicit operator string(ParamString self) => self.Value;
}