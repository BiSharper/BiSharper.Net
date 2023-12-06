using BiSharper.Rv.Param.Common.AST.Abstraction;

namespace BiSharper.Rv.Param.Common.AST.Value;

public readonly struct ParamFloat : IParamValue
{
    public float Value { get; }
    public IParamElement Parent { get; }

    public ParamFloat(float value, IParamElement parent)
    {
        Value = value;
        Parent = parent;
    }

    public static implicit operator float(ParamFloat self) => self.Value;
}