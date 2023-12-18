using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using BiSharper.Rv.Param.Serialization;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BiSharper.Rv.Param.Generator.Internal;

internal static class ParamSpecParser
{
    public static IParamGenerationSpec? Create
    (
        InterfaceDeclarationSyntax context,
        SemanticModel semanticModel,
        SymbolReference symbolReference,
        out IEnumerable<Diagnostic> diagnostics,
        CancellationToken cancellationToken
    )
    {
        var diagnosticsList = new List<Diagnostic>();
        var symbol = (INamedTypeSymbol?) semanticModel.GetDeclaredSymbol(context, cancellationToken);
        Debug.Assert(symbol != null);
        ParseSerializableAttribute(
            symbol!.GetAttributes().FirstOrDefault(f =>
                SymbolEqualityComparer.Default.Equals(f.AttributeClass, symbolReference.AttributeParamSerializable)
            ),
            out var serializationMode,
            out var generationMode
        );

        diagnostics = diagnosticsList;
        throw new NotImplementedException();
    }

    private static void ParseSerializableAttribute(AttributeData? attributeData,
        out ParamSerializationMode serializationMode, out ParamGenerationMode generationMode)
    {
        Debug.Assert(attributeData is not null);
        serializationMode = attributeData!.ConstructorArguments[0].Value as ParamSerializationMode? ??
                            ParamSerializationMode.ClassGeneration;
        generationMode = attributeData.ConstructorArguments[1].Value as ParamGenerationMode? ??
                         ParamGenerationMode.ReadWrite;
    }
}