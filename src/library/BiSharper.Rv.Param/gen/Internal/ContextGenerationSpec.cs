
using Microsoft.CodeAnalysis;

namespace BiSharper.Rv.Param.Generator.Internal;

internal readonly record struct ContextGenerationSpec(
    bool GenerateReadOnlyVariants
) : IParamGenerationSpec
{
    public void EmitSource(SourceProductionContext context, SymbolReference symbolReferences)
    {
        throw new System.NotImplementedException();
    }
}