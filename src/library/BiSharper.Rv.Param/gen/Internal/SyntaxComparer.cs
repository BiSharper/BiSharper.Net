using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BiSharper.Rv.Param.Generator.Internal;

internal class SyntaxComparer : IEqualityComparer<(TypeDeclarationSyntax, Compilation)>
{
    public static readonly SyntaxComparer Instance = new();

    public bool Equals((TypeDeclarationSyntax, Compilation) x, (TypeDeclarationSyntax, Compilation) y) =>
        x.Item1.Equals(y.Item1);

    public int GetHashCode((TypeDeclarationSyntax, Compilation) obj) =>
        obj.Item1.GetHashCode();
}