using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace BiSharper.Rv.Param.Generator.Internal;

internal static class Extensions
{
    public static IEnumerable<ISymbol> GetAllMembers(this INamedTypeSymbol symbol, bool withoutOverride = true)
    {
        if (symbol.BaseType != null)
        {
            foreach (var item in GetAllMembers(symbol.BaseType))
            {
                // override item already iterated in parent type
                if (!withoutOverride || !item.IsOverride)
                {
                    yield return item;
                }
            }
        }

        foreach (var item in symbol.GetMembers())
        {
            if (!withoutOverride || !item.IsOverride)
            {
                yield return item;
            }
        }
    }

    public static bool ContainsAttribute(this ISymbol symbol, INamedTypeSymbol attribute) =>
        symbol.GetAttributes().Any(x => SymbolEqualityComparer.Default.Equals(x.AttributeClass, attribute));

    public static bool EqualsUnconstructedGenericType(this INamedTypeSymbol left, INamedTypeSymbol right)
    {
        var l = left.IsGenericType ? left.ConstructUnboundGenericType() : left;
        var r = right.IsGenericType ? right.ConstructUnboundGenericType() : right;
        return SymbolEqualityComparer.Default.Equals(l, r);
    }

    public static AttributeData? GetAttribute(this ISymbol symbol, INamedTypeSymbol attribute) =>
        symbol.GetAttributes().FirstOrDefault(x => SymbolEqualityComparer.Default.Equals(x.AttributeClass, attribute));

}