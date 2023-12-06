using BiSharper.Rv.Param.AST.Abstraction;

namespace BiSharper.Rv.Param.AST.Value;

public readonly struct ParamFloat : IParamValue
{
    public float Value { get; }
    public ParamFloat(float value) => Value = value;
    public static explicit operator ParamFloat(float self) => new(self);
    public static implicit operator float(ParamFloat self) => self.Value;
}