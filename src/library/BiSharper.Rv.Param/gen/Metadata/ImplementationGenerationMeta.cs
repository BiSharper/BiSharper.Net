using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BiSharper.Rv.Param.Generator.Metadata;

internal readonly struct ImplementationGenerationMeta
{
    public StudGenerationMeta Stud { get; }
    public BaseTypeDeclarationSyntax Syntax { get; }
}