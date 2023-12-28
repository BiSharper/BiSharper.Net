using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BiSharper.Rv.Param.Generator.Internal;
using BiSharper.Rv.Param.Generator.Metadata;
using BiSharper.Rv.Param.Serialization;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BiSharper.Rv.Param.Generator;

[Generator(LanguageNames.CSharp)]
internal class ParamSerializerGenerator : IIncrementalGenerator
{
    private static readonly Dictionary<INamedTypeSymbol, ParamStudMeta> LocatedStuds = new(SymbolEqualityComparer.Default);

    public void Initialize(IncrementalGeneratorInitializationContext generationContext) => generationContext.RegisterSourceOutput(
        GatherStuds(generationContext),
        static (context, stud) => stud.EmitStud(context)
    );

    public static ParamStudMeta? LocateStud(INamedTypeSymbol symbol) =>
        LocatedStuds.TryGetValue(symbol, out var val) ? val : null;

    private static IncrementalValuesProvider<ParamStudMeta> GatherStuds(IncrementalGeneratorInitializationContext context)
    {
        return context.SyntaxProvider.ForAttributeWithMetadataName(
            SymbolReference.ParamSerializableAttributeFullname,
            predicate: static (node, token) => node is InterfaceDeclarationSyntax,
            transform: static (syntaxContext, token) =>
            {
                var interfaceSymbol = (INamedTypeSymbol)syntaxContext.TargetSymbol;
                var serializationMode =
                    interfaceSymbol.GetAttributes().First().ConstructorArguments.First().Value as ParamSerializationMode
                        ? ??
                    ParamSerializationMode.ClassGeneration;
                return LocatedStuds[interfaceSymbol] = new ParamStudMeta(syntaxContext, serializationMode, token);
            }
        );
    }
}