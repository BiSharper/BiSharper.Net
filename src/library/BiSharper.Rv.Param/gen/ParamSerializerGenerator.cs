using System.Threading;
using BiSharper.Rv.Param.Generator.Internal;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BiSharper.Rv.Param.Generator;

[Generator(LanguageNames.CSharp)]
public class ParamSerializerGenerator : IIncrementalGenerator
{

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var logProvider = context.AnalyzerConfigOptionsProvider
            .Select((configOptions, token) => (string?)null);
        var typeDeclarations = context.SyntaxProvider.ForAttributeWithMetadataName(
            ReferenceSymbols.ParamSerializableAttributePath,
            predicate: IsParamSerializable,
            transform: RetrieveTypeDeclarationSyntax
        );
        var source = typeDeclarations
            .Combine(context.CompilationProvider)
            .WithComparer(SyntaxComparer.Instance)
            .Combine(logProvider);

        context.RegisterSourceOutput(source, ParamSerializerEmitter.GenerateSourceOutput);
    }

    private static TypeDeclarationSyntax RetrieveTypeDeclarationSyntax(GeneratorAttributeSyntaxContext context, CancellationToken token) => (TypeDeclarationSyntax) context.TargetNode;


    private static bool IsParamSerializable(SyntaxNode node, CancellationToken token) =>
        node is ClassDeclarationSyntax or StructDeclarationSyntax or RecordDeclarationSyntax or InterfaceDeclarationSyntax;
}