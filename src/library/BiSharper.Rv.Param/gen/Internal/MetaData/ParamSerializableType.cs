using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BiSharper.Rv.Param.Serialization;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BiSharper.Rv.Param.Generator.Internal.MetaData;

internal readonly struct ParamSerializableType
{
    public readonly ReferenceSymbols Reference;
    public readonly INamedTypeSymbol Symbol;
    public ParamSerializationMode SerializationMode { get; }
    public string TypeName { get; }
    public IEnumerable<ParamSerializableMember> Members { get; }
    public bool IsValueType { get; }
    public bool IsReadOnly { get; }
    public bool IsUnmanagedType { get; }
    public bool IsRecord { get; }
    public bool IsInterfaceOrAbstract { get; }
    public bool IsReferenceStruct { get; }

    public ParamSerializableType(INamedTypeSymbol symbol, ReferenceSymbols reference)
    {
        Reference = reference;
        Symbol = symbol;
        SerializationMode = IdentifySerializationMode(symbol, reference);
        TypeName = Symbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
        Members = GatherMembers(symbol, reference);
        IsValueType = symbol.IsValueType;
        IsReadOnly = symbol.IsReadOnly;
        IsUnmanagedType = symbol.IsUnmanagedType;
        IsReferenceStruct = symbol.IsRefLikeType;
        IsInterfaceOrAbstract = symbol.IsAbstract;
        IsRecord = symbol.IsRecord;
    }

    public static IEnumerable<ParamSerializableMember> GatherMembers(INamedTypeSymbol typeSymbol, ReferenceSymbols reference)
    {
        foreach (var member in typeSymbol.GetAllMembers())
        {
            if(!ShouldUseMember(member, reference)) continue;
            Debug.Assert(member is not null);
            yield return ParamSerializableMember.Create(
                typeSymbol,
                IdentifyMemberName(member!, reference),
                typeSymbol.IsReadOnly,
                reference
            );
        }
    }


    public static string? IdentifyMemberName(ISymbol symbol, ReferenceSymbols reference) =>
        (string?) symbol.GetAttribute(reference.ParamMemberAttribute)?.ConstructorArguments.FirstOrDefault().Value;

    public static ParamSerializationMode IdentifySerializationMode(INamedTypeSymbol symbol, ReferenceSymbols reference)
    {
        var packableCtorArgs = symbol.GetAttribute(reference.ParamSerializableAttribute)?.ConstructorArguments;
        if (packableCtorArgs == null) return ParamSerializationMode.SkipGeneration;
        if (packableCtorArgs.Value.Length == 0) return ParamSerializationMode.ClassGeneration;
        var ctorValue = packableCtorArgs.Value[0];
        var generateType = ctorValue.Value ?? ParamSerializationMode.ClassGeneration;
        return (ParamSerializationMode)generateType;
    }

    public static bool ShouldUseMember(ISymbol? symbol, ReferenceSymbols reference) =>
        symbol is (IFieldSymbol or IPropertySymbol) and { IsStatic: false, IsImplicitlyDeclared: false }
        && !symbol.ContainsAttribute(reference.ParamIgnoreAttribute)
        && symbol.DeclaredAccessibility is Accessibility.Public &&
        (
            symbol is not IPropertySymbol property
            || (property.GetMethod != null || property.SetMethod == null)
            && !property.IsIndexer
        );

    public bool ValidateForGeneration(TypeDeclarationSyntax syntax, out Diagnostic? diagnostic)
    {
        if (SerializationMode == ParamSerializationMode.SkipGeneration)
        {
            diagnostic = null;
            return true;
        }

        throw new NotImplementedException();
    }


}