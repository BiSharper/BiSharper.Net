using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BiSharper.Rv.Param.Generator.Internal;

internal static class ParamSerializerEmitter
{

    public static void GenerateSourceOutput(SourceProductionContext context, ((TypeDeclarationSyntax syntax, Compilation compilation) source, string? logPath) arguments) =>
        GenerateSourceOutput(arguments.source.syntax, arguments.source.compilation, arguments.logPath, context);

    public static void GenerateSourceOutput(
        TypeDeclarationSyntax syntax,
        Compilation compilation,
        string? logPath,
        SourceProductionContext context
    )
    {
        var semanticModel = compilation.GetSemanticModel(syntax.SyntaxTree);
        if (ModelExtensions.GetDeclaredSymbol(semanticModel, syntax, context.CancellationToken) is not {} typeSymbol) return;
        {
            if (!LooksValid(syntax, typeSymbol, out var diagnostic))
            {
                context.ReportDiagnostic(diagnostic!);
                return;
            }
        }

        var reference = new ReferenceSymbols(compilation);


        throw new System.NotImplementedException();
    }


    private static bool LooksValid(BaseTypeDeclarationSyntax syntax, ISymbol symbol, out Diagnostic? diagnostic)
    {
        DiagnosticDescriptor? error = null;
        if (!IsPartial(syntax)) error = DiagnosticDescriptors.MustBePartial;
        if(IsNested(syntax)) error = DiagnosticDescriptors.NestedNotAllow;
        if (error is not null)
        {
            diagnostic = Diagnostic.Create(
                DiagnosticDescriptors.NestedNotAllow,
                syntax.Identifier.GetLocation(),
                symbol.Name
            );
            return false;
        }

        diagnostic = null;
        return true;
    }

    private static bool IsNested(SyntaxNode typeDeclaration) =>
        typeDeclaration.Parent is TypeDeclarationSyntax;

    private static bool IsPartial(MemberDeclarationSyntax syntax) =>
        syntax.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword));
}