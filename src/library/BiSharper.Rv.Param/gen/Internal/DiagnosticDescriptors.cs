using Microsoft.CodeAnalysis;

namespace BiSharper.Rv.Param.Generator.Internal;

internal class DiagnosticDescriptors
{
    private const string Category = "GenerateParamSerializer";
    private const string DescriptorIdPrefix = "ParamSerilizer-";

    public static readonly DiagnosticDescriptor MustBePartial = new(
        id: $"{DescriptorIdPrefix}1" ,
        title: "ParamSerializable object must be partial",
        messageFormat: "The ParamSerializable object '{0}' must be partial",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor NestedNotAllow = new(
        id: $"{DescriptorIdPrefix}1" ,
        title: "ParamSerializable object may not be a nested type",
        messageFormat: "The ParamSerializable object '{0}' may not be a nested type",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );
}