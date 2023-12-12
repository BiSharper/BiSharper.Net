using System;
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
    public ParamSerializableMember[] Members { get; }
    public bool IsValueType { get; }
    public bool IsUnmanagedType { get; }
    public bool IsRecord { get; }
    public bool IsInterfaceOrAbstract { get; }

    public ParamSerializableType(INamedTypeSymbol symbol, ReferenceSymbols reference)
    {
        Reference = reference;
        Symbol = symbol;
        SerializationMode = IdentifySerializationMode(symbol, reference);
        TypeName = Symbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);

        Members = Symbol.GetAllMembers()
            .Where(ShouldUseMember)
            .Select(s => ParamSerializableMember.Create(s, reference))
            .ToArray();
        IsValueType = symbol.IsValueType;
        IsUnmanagedType = symbol.IsUnmanagedType;
        IsInterfaceOrAbstract = symbol.IsAbstract;
        IsRecord = symbol.IsRecord;
    }

    public static ParamSerializationMode IdentifySerializationMode(INamedTypeSymbol symbol, ReferenceSymbols reference)
    {
        var packableCtorArgs = symbol.GetAttribute(reference.ParamSerializableAttribute)?.ConstructorArguments;
        if (packableCtorArgs == null) return ParamSerializationMode.SkipGeneration;
        if (packableCtorArgs.Value.Length == 0) return ParamSerializationMode.ClassGeneration;
        var ctorValue = packableCtorArgs.Value[0];
        var generateType = ctorValue.Value ?? ParamSerializationMode.ClassGeneration;
        return (ParamSerializationMode)generateType;
    }

    public bool ShouldUseMember(ISymbol symbol) =>
        symbol is (IFieldSymbol or IPropertySymbol) and { IsStatic: false, IsImplicitlyDeclared: false }
        && !symbol.ContainsAttribute(Reference.ParamIgnoreAttribute)
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