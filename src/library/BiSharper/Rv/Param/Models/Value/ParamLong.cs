using BiSharper.Rv.Param.Models.Statement;

namespace BiSharper.Rv.Param.Models.Value;

[ParamValue(ParamValueType.Long)]
public readonly struct ParamLong: IParamValue
{
    public required long Value { get; init; }
    public required IParamContext ParentContext { get; init; }

    public static implicit operator long(ParamLong self) => self.Value;
    
    public string ToText() => Value.ToString();
}