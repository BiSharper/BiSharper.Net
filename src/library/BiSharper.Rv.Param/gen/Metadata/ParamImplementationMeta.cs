using Microsoft.CodeAnalysis;

namespace BiSharper.Rv.Param.Generator.Metadata;

internal readonly record struct ParamImplementationMeta(ParamStudMeta ParamStud)
{
    public static ParamImplementationMeta? ParseImplementation(ParamStudMeta stud, SyntaxNode impl)
    {
        throw new System.NotImplementedException();
    }
}