using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using BiSharper.Rv.Param.Generator.Internal;
using BiSharper.Rv.Param.Serialization;
using Microsoft.CodeAnalysis;

namespace BiSharper.Rv.Param.Generator.Metadata;

internal readonly record struct ParamStudMeta
{
    public readonly ParamSerializationMode SerializationMode;
    public readonly ImmutableList<ParamStudMeta> ParentStuds;
    public readonly ImmutableList<ParamPropertyMeta> DefinedProperties;
    public readonly ImmutableList<ParamImplementationMeta> Implementations;

    public bool IsStudRoot => ParentStuds.IsEmpty;

    internal ParamStudMeta(GeneratorAttributeSyntaxContext context, ParamSerializationMode mode, CancellationToken token)
    {
        SerializationMode = mode;
        var symbolReference = new SymbolReference(context.SemanticModel.Compilation);
        var interfaceSymbol = (INamedTypeSymbol) context.TargetSymbol;

        ParentStuds = LocateParentStuds(interfaceSymbol, symbolReference);
        DefinedProperties = LocateProperties(interfaceSymbol, symbolReference);
        Implementations = LocateImplementations(context, this);
    }

    private static ImmutableList<ParamStudMeta> LocateParentStuds(ITypeSymbol interfaceSymbol, SymbolReference symbols)
    {
        var studs = new List<ParamStudMeta>();

        foreach (var symbol in interfaceSymbol.AllInterfaces.Where(it => it.ImplementsInterface(symbols.InterfaceParamStud)))
        {
            if (ParamSerializerGenerator.LocateStud(symbol) is { } stud) studs.Add(stud);
        }

        return studs.ToImmutableList();
    }

    private static ImmutableList<ParamImplementationMeta> LocateImplementations(
        GeneratorAttributeSyntaxContext context, ParamStudMeta owner)
    {
        var list = new List<ParamImplementationMeta>();
        foreach (var descendant in context.TargetNode.DescendantNodes())
            if(ParamImplementationMeta.ParseImplementation(owner, descendant) is { } impl) list.Add(impl);

        return list.ToImmutableList();
    }

    private static ImmutableList<ParamPropertyMeta> LocateProperties(INamespaceOrTypeSymbol symbol, SymbolReference reference)
    {
        var list = new List<ParamPropertyMeta>();
        foreach (var member in symbol.GetMembers())
        {
            if (member.GetAttributes().FirstOrDefault(f =>
                SymbolEqualityComparer.Default.Equals(f.AttributeClass, reference.AttributeParamProperty)) is not { } paramAttribute
            ) continue;

            if (member is not IPropertySymbol propertySymbol)
            {
                continue;
            }

            if (propertySymbol.SetMethod is not null)
            {

                continue;
            }

            if (ParamPropertyMeta.Parse(propertySymbol, paramAttribute) is not { } property)
            {

                continue;
            }
            list.Add(property);
        }

        return list.ToImmutableList();
    }
}