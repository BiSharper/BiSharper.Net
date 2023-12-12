using System;
using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;

namespace BiSharper.Rv.Param.Generator.Internal.MetaData;

internal readonly struct ParamSerializableMember
{
    public readonly ISymbol Symbol;
    public readonly ReferenceSymbols Reference;
    public string Name { get; }
    public ITypeSymbol MemberType { get; }
    public bool IsSettable { get; }
    public bool IsRef { get; }
    public bool IsAssignable { get; }
    public bool IsProperty { get; private init; }
    public bool IsRequired { get; private init;  }

    public bool IsField
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => !IsProperty;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private init => IsProperty = !value;
    }


    public static ParamSerializableMember Create(ISymbol symbol, ReferenceSymbols reference) => symbol switch
    {
        IFieldSymbol f => new ParamSerializableMember(f, reference),
        IPropertySymbol p => new ParamSerializableMember(p, reference),
        _ => throw new Exception("Symbol must be field or property!")
    };

    private ParamSerializableMember(IFieldSymbol field, ReferenceSymbols reference)
    {
        Symbol = field;
        Name = field.Name;
        Reference = reference;
        MemberType = field.Type;
        IsField = true;
        IsRef = field.RefKind is RefKind.Ref or RefKind.RefReadOnly;
        IsAssignable = (IsSettable = !field.IsReadOnly) && !field.IsRequired;
        IsRequired = field.ContainsAttribute(reference.ParamRequiredAttribute);
    }

    private ParamSerializableMember(IPropertySymbol property, ReferenceSymbols reference)
    {
        Symbol = property;
        Name = property.Name;
        Reference = reference;
        MemberType = property.Type;
        IsField = false;
        IsAssignable = (IsSettable = !property.IsReadOnly) && property is { IsRequired: false, SetMethod.IsInitOnly: false };
        IsRef = property.RefKind is RefKind.Ref or RefKind.RefReadOnly;
        IsRequired = property.ContainsAttribute(reference.ParamRequiredAttribute);
    }
}