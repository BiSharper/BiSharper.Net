using BiSharper.Rv.Param.Common.AST.Abstraction;
using BiSharper.Rv.Param.Common.AST.Statement;

namespace BiSharper.Rv.Param.Common.AST.Value;

public readonly struct ParamString : IParamValue
{
    public string Value { get; }
    public IParamElement Parent { get; }

    public ParamString(string value, IParamElement parent)
    {
        Value = value;
        Parent = parent;
    }

    public static implicit operator string(ParamString self) => self.Value;
}