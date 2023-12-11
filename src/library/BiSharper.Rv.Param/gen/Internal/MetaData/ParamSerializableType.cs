using System;
using System.Linq;
using BiSharper.Rv.Param.Serialization;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BiSharper.Rv.Param.Generator.Internal.MetaData;

internal readonly struct ParamSerializableType
{
    private readonly ReferenceSymbols _reference;
    public readonly INamedTypeSymbol Symbol;
    public ParamSerializationMode SerializationMode { get; }
    public string TypeName { get; }
    public ParamSerializableMember[] Members { get; }

    public ParamSerializableType(INamedTypeSymbol typeSymbol, ReferenceSymbols reference)
    {
        _reference = reference;
        Symbol = typeSymbol;
        SerializationMode = IdentifySerializationMode(typeSymbol, reference);
        TypeName = Symbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);


        Members = Symbol.GetAllMembers()
            .Where(ShouldUseMember)
            .Select(symbol => ParamSerializableMember.Create(symbol, reference))
            .ToArray();
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
        && !symbol.ContainsAttribute(_reference.ParamIgnoreAttribute)
        && symbol.DeclaredAccessibility is Accessibility.Public &&
        (
            symbol is not IPropertySymbol property
            || (property.GetMethod != null || property.SetMethod == null)
            && !property.IsIndexer
        );

    public bool ValidateForGeneration(TypeDeclarationSyntax syntax, out Diagnostic? diagnostic)
    {

        throw new NotImplementedException();
    }
}