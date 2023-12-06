using BiSharper.Rv.Param.Common.AST.Abstraction;
using BiSharper.Rv.Param.Common.AST.Statement;

namespace BiSharper.Rv.Param.Common.AST.Value;

public readonly struct ParamLong : IParamValue
{
    public long Value { get; }
    public IParamElement Parent { get; }

    public ParamLong(long value, IParamElement parent)
    {
        Value = value;
        Parent = parent;
    }

    public static implicit operator long(ParamLong self) => self.Value;
}