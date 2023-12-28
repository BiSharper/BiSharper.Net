using System;
using BiSharper.Rv.Param.Generator.Metadata;
using Microsoft.CodeAnalysis;

namespace BiSharper.Rv.Param.Generator.Internal;

internal static partial class ParamSerializerEmitter
{
    private static void EmitImplementation(this ParamImplementationMeta impl, SourceProductionContext context)
    {
        throw new System.NotImplementedException();
    }

    private static void EmitHelpers(this ParamStudMeta stud, SourceProductionContext context)
    {
        throw new NotImplementedException();
    }

    public static void EmitStud(this ParamStudMeta stud, SourceProductionContext context)
    {
        stud.EmitHelpers(context);
        stud.EmitImplementations(context);
    }

    private static void EmitImplementations(this ParamStudMeta stud, SourceProductionContext context)
    {
        foreach (var impl in stud.Implementations) impl.EmitImplementation(context);
    }

}