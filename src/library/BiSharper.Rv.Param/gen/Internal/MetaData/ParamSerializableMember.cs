using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.CodeAnalysis;

namespace BiSharper.Rv.Param.Generator.Internal.MetaData;

internal readonly struct ParamSerializableMember
{
    public readonly ISymbol Symbol;
    public readonly ReferenceSymbols Reference;
    public ParamMemberKind Kind { get; }
    public string Name { get; }
    public string MemberName { get; }
    public ITypeSymbol MemberType { get; }
    public bool IsSettable { get; }
    public bool IsReadOnly { get; }
    public bool IsRef { get; }
    public bool IsAssignable { get; }
    public bool IsProperty { get; private init; }
    public bool IsRequired { get; private init; }
    public bool IsParamApi { get => _isParamApi; private init => _isParamApi = value; }
    private readonly bool _isParamApi;

    public bool IsField
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => !IsProperty;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private init => IsProperty = !value;
    }


    public static ParamSerializableMember Create(ISymbol symbol, string? memberName, bool isReadOnly, ReferenceSymbols reference) => symbol switch
    {
        IFieldSymbol f => new ParamSerializableMember(f, memberName, isReadOnly, reference),
        IPropertySymbol p => new ParamSerializableMember(p, memberName, isReadOnly, reference),
        _ => throw new Exception("Symbol must be field or property!")
    };

    private ParamSerializableMember(IFieldSymbol field, string? memberName, bool isReadOnly, ReferenceSymbols reference)
    {
        Symbol = field;
        Name = field.Name;
        MemberName = memberName ?? Name;
        Reference = reference;
        MemberType = field.Type;
        IsField = true;
        IsRef = field.RefKind is RefKind.Ref or RefKind.RefReadOnly;
        IsReadOnly = isReadOnly | field.IsReadOnly;
        IsAssignable = !IsReadOnly && !field.IsRequired;
        IsRequired = field.ContainsAttribute(reference.ParamRequiredAttribute);
        Kind = ParseKind(ref _isParamApi);
    }

    private ParamSerializableMember(IPropertySymbol property, string? memberName, bool isReadOnly, ReferenceSymbols reference)
    {
        Symbol = property;
        Name = property.Name;
        MemberName = memberName ?? Name;
        Reference = reference;
        MemberType = property.Type;
        IsField = false;
        IsReadOnly = isReadOnly | property.IsReadOnly;
        IsAssignable = (IsSettable = !property.IsReadOnly) && property is { IsRequired: false, SetMethod.IsInitOnly: false };
        IsRef = property.RefKind is RefKind.Ref or RefKind.RefReadOnly;
        IsRequired = property.ContainsAttribute(reference.ParamRequiredAttribute);
        Kind = ParseKind(ref _isParamApi);
    }

    private ParamMemberKind ParseKind(ref bool isParamApi)
    {
        if (MemberType.SpecialType is
                SpecialType.System_Object or
                SpecialType.System_Array or
                SpecialType.System_Delegate or
                SpecialType.System_MulticastDelegate
            || MemberType.TypeKind is TypeKind.Delegate
           ) goto EndCase;
        if (Reference.IsParamObject(MemberType))
            return ParamMemberKind.Object;
        if (MemberType.TypeKind == TypeKind.Enum)
            return Marshal.SizeOf(MemberType) <= 4 ? ParamMemberKind.Integer : ParamMemberKind.Double;
        switch (MemberType.SpecialType)
        {
            case SpecialType.System_String:
                return ParamMemberKind.String;
            case SpecialType.System_Byte or SpecialType.System_Int32 or SpecialType.System_Int16:
                return ParamMemberKind.Integer;
            case SpecialType.System_Single:
                return ParamMemberKind.Float;
            case SpecialType.System_Double:
                return ParamMemberKind.Double;
        }

        EndCase:
        return ParamMemberKind.NonSerializable;
    }


}