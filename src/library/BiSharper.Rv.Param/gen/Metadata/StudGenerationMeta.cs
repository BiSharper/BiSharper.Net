using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BiSharper.Rv.Param.Generator.Metadata;

internal class StudGenerationMeta
{

    public readonly InterfaceDeclarationSyntax Interface;
    public readonly ImmutableList<PropertyGenerationMeta> DefinedProperties;
    public readonly ImmutableList<ImplementationGenerationMeta> Implementations;

    public StudGenerationMeta(InterfaceDeclarationSyntax sInterface)
    {
        Interface = sInterface;
    }

    public void EmitSource(SourceProductionContext context)
    {
        throw new System.NotImplementedException();
    }
}