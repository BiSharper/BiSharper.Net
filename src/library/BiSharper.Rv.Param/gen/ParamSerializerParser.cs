using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using BiSharper.Rv.Param.Generator.Internal;
using BiSharper.Rv.Param.Generator.Metadata;
using BiSharper.Rv.Param.Serialization;
using Microsoft.CodeAnalysis;

namespace BiSharper.Rv.Param.Generator;

internal static class ParamSerializerParser
{
    public static StudGenerationMeta ParseStud(
        GeneratorAttributeSyntaxContext context,
        CancellationToken cancellationToken
    )
    {
        var interfaceSymbol = (INamedTypeSymbol) context.TargetSymbol;
        var symbolReference = SymbolReference.ForCompilation(context.SemanticModel.Compilation);
        var serializationMode =
            interfaceSymbol.GetAttributes().First().ConstructorArguments.First().Value as ParamSerializationMode? ??
            ParamSerializationMode.ClassGeneration;
        var definedProperties = (ImmutableList<PropertyGenerationMeta>)LocateParamProperties();

        var implementations = (ImmutableList<ImplementationGenerationMeta>) context.TargetNode.DescendantNodes().Select(ParseImplementation);

        return new StudGenerationMeta(serializationMode, definedProperties, implementations);

        IEnumerable<PropertyGenerationMeta> LocateParamProperties()
        {
            foreach (var member in interfaceSymbol.GetMembers())
            {
                if (member.GetAttributes().FirstOrDefault(f =>
                        SymbolEqualityComparer.Default.Equals(f.AttributeClass, symbolReference.AttributeParamProperty))
                    is not { } paramAttribute) continue;
                var paramName = paramAttribute.ConstructorArguments[0].Value as string;
                if (string.IsNullOrWhiteSpace(paramName)) paramName = interfaceSymbol.Name;

            }
            throw new NotImplementedException();
        }
    }

    private static ImplementationGenerationMeta ParseImplementation(SyntaxNode arg)
    {
        throw new System.NotImplementedException();
    }
}