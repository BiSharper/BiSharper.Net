using BiSharper.Rv.Param.Common.AST.Abstraction;
using BiSharper.Rv.Param.Common.AST.Statement;

namespace BiSharper.Rv.Param.Common.AST.Value;

public readonly struct ParamInteger : IParamValue
{
    public int Value { get; }
    public IParamElement Parent { get; }

    public ParamInteger(int value, IParamElement parent)
    {
        Value = value;
        Parent = parent;
    }

    public static implicit operator int(ParamInteger self) => self.Value;
}