using System.Collections.Generic;
using System.Threading;
using BiSharper.Rv.Param.Generator.Internal;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BiSharper.Rv.Param.Generator;

[Generator(LanguageNames.CSharp)]
internal class ParamSerializerGenerator : IIncrementalGenerator
{
    public const string SourceGenerationSpecTrackingName = "SourceGenerationSpec";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var symbolReference = context.CompilationProvider
            .Select((compilation, _) => new SymbolReference(compilation));
        var generationSpecs = context.SyntaxProvider.ForAttributeWithMetadataName(
            SymbolReference.ParamSerializableAttributeFullname,
            predicate: IsParamSerializable,
            transform: static (transformationContext, _) =>
                ((InterfaceDeclarationSyntax)transformationContext.TargetNode, transformationContext.SemanticModel)
        ).Combine(symbolReference)
        .Select(CreateSpec)
        .WithTrackingName(SourceGenerationSpecTrackingName);

        context.RegisterSourceOutput(generationSpecs, ReportDiagnosticsAndEmit);
    }

    private static void ReportDiagnosticsAndEmit
    (
        SourceProductionContext context,
        (
            IParamGenerationSpec? GenerationSpec,
            SymbolReference SymbolReferences,
            IEnumerable<Diagnostic> Diagnostics
        ) input
    )
    {

        ReportDiagnostics(context, input.Diagnostics);

        if (input.GenerationSpec is { } generationSpec) generationSpec.EmitSource(context, input.SymbolReferences);
    }

    private static void EmitGenerationSpec
    (
        IParamGenerationSpec? generationSpec,
        SourceProductionContext context,
        SymbolReference symbolReferences
    ) => generationSpec?.EmitSource(context, symbolReferences);

    private static void ReportDiagnostics(SourceProductionContext context, IEnumerable<Diagnostic> diagnostics)
        { foreach (var diagnostic in diagnostics) context.ReportDiagnostic(diagnostic); }

    private static (IParamGenerationSpec?, SymbolReference, IEnumerable<Diagnostic>) CreateSpec(
        ((InterfaceDeclarationSyntax Syntax, SemanticModel SemanticModel) Input, SymbolReference SymbolReference) tuple,
        CancellationToken cancellationToken
    ) =>
    (
        ParamSpecParser.Create
        (
            tuple.Input.Syntax,
            tuple.Input.SemanticModel,
            tuple.SymbolReference,
            out var diagnostics,
            cancellationToken
        ),
        tuple.SymbolReference,
        diagnostics
    );

    private static bool IsParamSerializable(SyntaxNode node, CancellationToken token) =>
        node is InterfaceDeclarationSyntax;
}