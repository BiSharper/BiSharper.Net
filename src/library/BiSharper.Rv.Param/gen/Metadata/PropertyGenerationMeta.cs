using System.IO;
using BiSharper.Rv.Param.Generator.Internal;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BiSharper.Rv.Param.Generator.Metadata;

internal readonly struct PropertyGenerationMeta
{
    private StudGenerationMeta Stud { get; }
    private PropertyDeclarationSyntax Syntax { get; }
    private BaseParameterType ParamType { get; }
    private string PropertyName { get; }
    private bool Required { get; }

}