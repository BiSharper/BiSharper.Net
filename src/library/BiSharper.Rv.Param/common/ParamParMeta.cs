namespace BiSharper.Rv.Param.Common;

[Flags]
public enum ParamOperatorType : byte
{
    Assign,
    AdditiveAssign,
    SubtractiveAssign,

    ArrayOperations = Assign | AdditiveAssign | SubtractiveAssign
}

public enum ParamValueType
{
    Array, Expression, Float, Integer, Long, String
}

public readonly struct ParamParMeta(string name, ParamValueType type, ParamOperatorType op = ParamOperatorType.Assign)
{

    public string Name { get; init; } = name;
    public ParamValueType ValueType { get; init; } = type;
    public ParamOperatorType Operator { get; init; } = op;

    public IParamValue AssertValid(IParamValue? value)
    {
        if (!Validate(value)) throw new Exception($"Identifier {Name} is not valid.");
        return value!;
    }

    public static bool ValidateIdentifier(string id) => id.First() switch
    {
        { } first when char.IsDigit(first) => false,
        _ => id.All(c => char.IsLetterOrDigit(c) || c == '_')
    };

    public bool Validate(IParamValue? value) =>
        value is not null && value.SupportsOperator(op) && ValidateIdentifier(Name);

}