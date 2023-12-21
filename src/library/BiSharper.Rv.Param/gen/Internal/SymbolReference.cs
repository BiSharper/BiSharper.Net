using System;
using System.Collections.Generic;
using System.Linq;
using BiSharper.Rv.Param.AST.Abstraction;
using BiSharper.Rv.Param.AST.Statement;
using BiSharper.Rv.Param.AST.Value;
using BiSharper.Rv.Param.AST.Value.Enumerable;
using BiSharper.Rv.Param.AST.Value.Numeric;
using BiSharper.Rv.Param.Serialization.Attributes;
using BiSharper.Rv.Param.Serialization.Context;
using Microsoft.CodeAnalysis;
// ReSharper disable InconsistentNaming

namespace BiSharper.Rv.Param.Generator.Internal;

internal readonly struct SymbolReference
{
    private readonly Compilation _compilation;

    private static readonly Dictionary<Compilation, SymbolReference> _symbolReferences =
        new Dictionary<Compilation, SymbolReference>();

    // System Types
    public INamedTypeSymbol? InterfaceEnumerableOfTType { get; private init; }
    public INamedTypeSymbol? InterfaceListOfTType { get; private init; }
    public INamedTypeSymbol? InterfaceReadOnlyListOfTType { get; private init; }
    public INamedTypeSymbol? ListOfTType { get; private init; }
    public INamedTypeSymbol StringType { get; private init; }
    public INamedTypeSymbol FloatType { get; private init; }
    public INamedTypeSymbol DoubleType { get; private init; }
    public INamedTypeSymbol IntegerType { get; private init; }

    // API Types
    public INamedTypeSymbol? InterfaceParamSerializableContextType { get; private init; }
    public INamedTypeSymbol? InterfaceParamClass { get; private init; }
    public INamedTypeSymbol? InterfaceParamContext { get; private init; }
    public INamedTypeSymbol? InterfaceParamListOfTType { get; private init; }
    public INamedTypeSymbol? InterfaceParamString { get; private init; }
    public INamedTypeSymbol? InterfaceParamList { get;  private init; }
    public INamedTypeSymbol? InterfaceParamInteger { get; private init; }
    public INamedTypeSymbol? InterfaceParamDouble { get; private init; }
    public INamedTypeSymbol? InterfaceParamExpression { get; private init; }
    public INamedTypeSymbol? InterfaceParamFloat { get; private init; }
    public INamedTypeSymbol? AttributeParamProperty { get; private init; }
    public INamedTypeSymbol? AttributeParamSerializable { get; private init; }
    public INamedTypeSymbol? ParamListOfTType { get; private init; }
    public INamedTypeSymbol? ParamReadOnlyListOfTType { get; private init; }
    public INamedTypeSymbol? ParamDoubleType { get; private init; }
    public INamedTypeSymbol? ParamIntegerType { get; private init; }
    public INamedTypeSymbol? ParamStringType { get; private init; }
    public INamedTypeSymbol? ParamExpressionType { get; private init; }
    public INamedTypeSymbol? ParamFloatType { get; private init; }
    public const string ParamSerializableAttributeFullname = "BiSharper.Rv.Param.Serialization.Attributes.ParamSerializableAttribute";

    private SymbolReference(Compilation compilation)
    {
        _compilation = compilation;

        InterfaceEnumerableOfTType = GetOrResolveType(typeof(IEnumerable<>));
        InterfaceListOfTType = GetOrResolveType(typeof(IList<>));
        InterfaceReadOnlyListOfTType = GetOrResolveType(typeof(IReadOnlyList<>));
        ListOfTType = GetOrResolveType(typeof(List<>));
        StringType = compilation.GetSpecialType(SpecialType.System_String);
        FloatType = compilation.GetSpecialType(SpecialType.System_Single);
        DoubleType = compilation.GetSpecialType(SpecialType.System_Double);
        IntegerType = compilation.GetSpecialType(SpecialType.System_Int32);

        InterfaceParamSerializableContextType = GetOrResolveType(typeof(IParamSerializableContext));
        InterfaceParamContext = GetOrResolveType(typeof(IParamContext));
        InterfaceParamClass = GetOrResolveType(typeof(IParamClass));
        InterfaceParamListOfTType = GetOrResolveType(typeof(IParamArray<>));
        InterfaceParamList = GetOrResolveType(typeof(IParamArray<IParamValue>));
        InterfaceParamString = GetOrResolveType(typeof(IParamString));
        InterfaceParamExpression = GetOrResolveType(typeof(IParamExpression));
        InterfaceParamFloat = GetOrResolveType(typeof(IParamFloat));
        InterfaceParamDouble = GetOrResolveType(typeof(IParamDouble));
        InterfaceParamInteger = GetOrResolveType(typeof(IParamInteger));
        AttributeParamProperty = GetOrResolveType(typeof(ParamPropertyAttribute));
        AttributeParamSerializable = _compilation.GetTypeByMetadataName(ParamSerializableAttributeFullname);
        ParamListOfTType = GetOrResolveType(typeof(ParamList<>));
        ParamReadOnlyListOfTType = GetOrResolveType(typeof(ParamReadOnlyList<>));
        ParamDoubleType = GetOrResolveType(typeof(ParamDouble));
        ParamIntegerType = GetOrResolveType(typeof(ParamInteger));
        ParamStringType = GetOrResolveType(typeof(ParamString));
        ParamExpressionType = GetOrResolveType(typeof(ParamExpression));
        ParamFloatType = GetOrResolveType(typeof(ParamFloat));
    }

    public BaseParameterType? ValidatePropertyType(
        ITypeSymbol type,
        out int matchCount
    )
    {
        BaseParameterType? determinedType = null;
        var i = 0;

        foreach (var iInterface in type.AllInterfaces)
        {
            determinedType = iInterface switch
            {
                _ when SymbolEqualityComparer.Default.Equals(iInterface, InterfaceParamInteger) =>
                    AssignType(BaseParameterType.Integer),
                _ when SymbolEqualityComparer.Default.Equals(iInterface, InterfaceParamDouble) =>
                    AssignType(BaseParameterType.Double),
                _ when SymbolEqualityComparer.Default.Equals(iInterface, InterfaceParamFloat) =>
                    AssignType(BaseParameterType.Float),
                _ when SymbolEqualityComparer.Default.Equals(iInterface, InterfaceParamString) =>
                    AssignType(BaseParameterType.String),
                _ when SymbolEqualityComparer.Default.Equals(iInterface, InterfaceParamExpression) =>
                    AssignType(BaseParameterType.Expression),
                _ => determinedType
            };
            continue;
            BaseParameterType AssignType(BaseParameterType t) {i++; return t;}
        }

        matchCount = i;
        return determinedType;
    }

    private INamedTypeSymbol? GetOrResolveType(Type type) => GetOrResolveType(type.FullName!);

    private INamedTypeSymbol? GetOrResolveType(string fullyQualifiedName) => _compilation.GetTypeByMetadataName(fullyQualifiedName);

    public static SymbolReference ForCompilation(Compilation compilation)
    {
        if (!_symbolReferences.TryGetValue(compilation, out var symbolReference))
        {
            symbolReference = new SymbolReference(compilation);
        }
        return symbolReference;
    }
}