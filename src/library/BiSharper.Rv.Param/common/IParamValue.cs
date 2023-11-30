
namespace BiSharper.Rv.Param.Common;

public interface IParamValue : IParamElement
{
    public object ValueUnwrapped { get; }

    public string ToText();
}

[AttributeUsage(AttributeTargets.Struct)]
internal class ParamValueAttribute(ParamValueType type, ParamOperatorType supportedOperators = ParamOperatorType.Assign) : Attribute
{
    public ParamValueType ValueType { get; } = type;
    public ParamOperatorType SupportedOperators { get; } = supportedOperators;
}

public static class ParamValueExtension
{
    private static readonly Dictionary<Type, ParamValueAttribute> ValueMeta = new();

    public static bool SupportsOperator(this IParamValue value, ParamOperatorType op) =>
        (GetMetadata(value).SupportedOperators & op) == op;

    private static ParamValueAttribute GetMetadata(this IParamValue value)
    {
        var valueType = value.GetType();
        _ = ValueMeta.TryGetValue(valueType, out var attribute);
        if (attribute is not null) return attribute;
        var metadata = Attribute.GetCustomAttributes(valueType).OfType<ParamValueAttribute>().FirstOrDefault() ?? throw new Exception("Class must have the ParamValueAttribute.");
        ValueMeta[valueType] = attribute = metadata;
        return attribute;
    }

    public static ParamValueType GetValueType(this IParamValue value) =>
        value.GetMetadata().ValueType;
}