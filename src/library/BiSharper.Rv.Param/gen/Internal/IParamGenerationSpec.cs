using Microsoft.CodeAnalysis;

namespace BiSharper.Rv.Param.Generator.Internal;

internal interface IParamGenerationSpec
{
    bool GenerateReadOnlyVariants { get; }


    void EmitSource(SourceProductionContext context, SymbolReference symbolReferences);
}