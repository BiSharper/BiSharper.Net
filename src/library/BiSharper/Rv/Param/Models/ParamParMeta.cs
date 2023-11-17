using System.Text.RegularExpressions;
using BiSharper.Rv.Param.Models.Value;

namespace BiSharper.Rv.Param.Models;

public readonly partial struct ParamParMeta(string name, ParamValueType type, ParamOperatorType op = ParamOperatorType.Assign)
{
    [GeneratedRegex("^[a-zA-Z_][a-zA-Z0-9_]*$", RegexOptions.Compiled)]
    private static partial Regex IdentifierRegex();

    public string Name { get; init; } = name;
    public ParamValueType ValueType { get; init; } = type;
    public ParamOperatorType Operator { get; init; } = op;

    public override int GetHashCode() => HashCode.Combine(Name.GetHashCode(), ValueType.GetHashCode());

    public IParamValue AssertValid(IParamValue? value)
    {
        if (!Validate(value)) throw new Exception($"Identifier {Name} is not valid.");
        return value!;
    }

    public bool Validate(IParamValue? value) =>
        value is not null && value.SupportsOperator(op) && IdentifierRegex().IsMatch(Name);

}