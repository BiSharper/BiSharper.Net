using System;
using BiSharper.Rv.Param.Generator.Internal;
using Microsoft.CodeAnalysis;

namespace BiSharper.Rv.Param.Generator.Metadata;

internal readonly record struct ParamPropertyMeta
{
    public readonly ParamStudMeta ParamStud;
    public readonly BaseParameterType ParamType;
    public readonly string ParamName;
    public readonly bool Required { get; }


    public static ParamPropertyMeta? Parse(IPropertySymbol symbol, AttributeData attributeData)
    {
        // if(symbol.GetMethod is not null || symbol.SetMethod)
        var paramName = attributeData.ConstructorArguments[0].Value as string;
        if (string.IsNullOrWhiteSpace(paramName)) paramName = symbol.Name;

        throw new NotImplementedException();
    }
}