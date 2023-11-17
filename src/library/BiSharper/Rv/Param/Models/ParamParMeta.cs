using BiSharper.Rv.Param.Models.Value;

namespace BiSharper.Rv.Param.Models;

public readonly struct ParamParMeta(string name, ParamValueType type, ParamOperatorType op = ParamOperatorType.Assign)
{
    public string Name { get; init; } = name;
    public ParamValueType Type { get; init; } = type;
    public ParamOperatorType Operator { get; init; } = op;
}