using System;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace BiSharper.Rv.Param.Generator.Internal;

internal readonly struct ReferenceSymbols
{
    public const string ParamSerializableAttributePath =
        "BiSharper.Rv.Param.Serialization.Attribute.ParamSerializableAttribute";
    public Compilation Compilation { get; }

    public INamedTypeSymbol ParamMemberAttribute =>
        GetTypeByMetadataName("BiSharper.Rv.Param.Serialization.Attribute.ParamMemberAttribute");
    public INamedTypeSymbol ParamRequiredAttribute =>
        GetTypeByMetadataName("BiSharper.Rv.Param.Serialization.Attribute.ParamRequiredAttribute");
    public INamedTypeSymbol ParamIgnoreAttribute =>
        GetTypeByMetadataName("BiSharper.Rv.Param.Serialization.Attribute.ParamIgnoreAttribute");
    public INamedTypeSymbol ParamSerializableAttribute =>
        GetTypeByMetadataName(ParamSerializableAttributePath);
    public INamedTypeSymbol ParamFloatStruct =>
        GetTypeByMetadataName("BiSharper.Rv.Param.AST.Value.ParamFloat");
    public INamedTypeSymbol ParamIntegerStruct =>
        GetTypeByMetadataName("BiSharper.Rv.Param.AST.Value.ParamInteger");
    public INamedTypeSymbol ParamContextInterface =>
        GetTypeByMetadataName("BiSharper.Rv.Param.AST.Abstraction.IParamContext");


    public INamedTypeSymbol ParamSerializableInterface =>
        GetTypeByMetadataName("BiSharper.Rv.Param.Serialization.IParamSerializable");


    private INamedTypeSymbol GetTypeByMetadataName(string metadataName) => Compilation.GetTypeByMetadataName(metadataName)
        ?? throw new InvalidOperationException($"Type {metadataName} is not found in compilation.");


    public ReferenceSymbols(Compilation compilation)
    {
        Compilation = compilation;
    }

    public bool IsParamObject(ITypeSymbol memberType)
    {
        foreach (var @interface in memberType.AllInterfaces)
        {
            if (SymbolEqualityComparer.Default.Equals(@interface, ParamContextInterface))
            {
                return true;
            }
        }

        return false;
    }
}