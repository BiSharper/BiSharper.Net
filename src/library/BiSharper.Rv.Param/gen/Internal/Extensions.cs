using System.Linq;
using Microsoft.CodeAnalysis;

namespace BiSharper.Rv.Param.Generator.Internal;

internal static class Extensions
{
    public static bool ImplementsInterface(this INamedTypeSymbol currentInterface, INamedTypeSymbol targetInterface) =>
        SymbolEqualityComparer.Default.Equals(currentInterface, targetInterface) || Enumerable.Any(currentInterface.AllInterfaces, baseInterface => baseInterface.ImplementsInterface(targetInterface));
}