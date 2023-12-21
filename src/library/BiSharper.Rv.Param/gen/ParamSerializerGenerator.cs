using BiSharper.Rv.Param.Generator.Internal;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BiSharper.Rv.Param.Generator;

[Generator(LanguageNames.CSharp)]
internal class ParamSerializerGenerator : IIncrementalGenerator
{
    public const string SourceGenerationSpecTrackingName = "SourceGenerationSpec";

    public void Initialize(IncrementalGeneratorInitializationContext generationContext)
    {
        var identifiedImplementations =
            generationContext.SyntaxProvider.ForAttributeWithMetadataName(
                SymbolReference.ParamSerializableAttributeFullname,
                predicate: static (node, _) => node is InterfaceDeclarationSyntax,
                transform: ParamSerializerParser.ParseStud
            );
        generationContext.RegisterSourceOutput(identifiedImplementations, ParamSerializerEmitter.EmitSource);
    }
}