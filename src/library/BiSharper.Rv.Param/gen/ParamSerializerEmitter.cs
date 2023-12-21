using BiSharper.Rv.Param.Generator.Metadata;
using Microsoft.CodeAnalysis;

namespace BiSharper.Rv.Param.Generator;

internal static class ParamSerializerEmitter
{
    private static void EmitStud(this StudGenerationMeta stud, SourceProductionContext context)
    {
        throw new System.NotImplementedException();
    }

    private static void EmitImplementation(this ImplementationGenerationMeta impl, SourceProductionContext context)
    {
        throw new System.NotImplementedException();
    }

    public static void EmitSource(SourceProductionContext context, StudGenerationMeta stud)
    {
        stud.EmitStud(context);
        stud.EmitImplementations(context);
    }

    private static void EmitImplementations(this StudGenerationMeta stud, SourceProductionContext context)
    {
        foreach (var impl in stud.Implementations) impl.EmitImplementation(context);
    }
}