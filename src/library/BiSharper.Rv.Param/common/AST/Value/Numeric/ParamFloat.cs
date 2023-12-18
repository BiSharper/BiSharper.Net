namespace BiSharper.Rv.Param.AST.Value.Numeric;

public interface IParamFloat : IParamValue
{
    public int Value { get; }
}

public readonly struct ParamFloat : IParamValue
{
    public float Value { get; }
    public ParamFloat(float value) => Value = value;
    public static explicit operator ParamFloat(float self) => new(self);
    public static implicit operator float(ParamFloat self) => self.Value;
}