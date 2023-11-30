namespace BiSharper.Rv.Param.Common;

public readonly struct ParamParameter(string name, ParamOperatorType op, IParamValue value, ParamContext parent) : IParamStatement
{
    public string Name { get; } = name;
    public ParamOperatorType Operator { get; } = op;
    public IParamValue Value { get; } = value;
    public ParamContext ParentContext { get; } = parent;

    public static bool ValidateIdentifier(string id) => id.First() switch
    {
        var first when char.IsDigit(first) => false,
        _ => id.All(c => char.IsLetterOrDigit(c) || c == '_')
    };



    public bool Valid => Value.SupportsOperator(Operator) && ValidateIdentifier(Name);

}

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