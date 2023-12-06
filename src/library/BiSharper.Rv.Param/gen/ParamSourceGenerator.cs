using Microsoft.CodeAnalysis;

namespace BiSharper.Rv.Param.Generator;

[Generator(LanguageNames.CSharp)]
public class ParamSerializerGenerator : IIncrementalGenerator
{

    public const string AttributesNamespace = "BiSharper.Rv.Param.Common.Attributes.";
    // public const string ParamSerializableAttributeFullName = AttributesNamespace + nameof(ParamSerializableAttribute);
    // public const string ParamIncludeAttributeFullName = AttributesNamespace + nameof(ParamIncludeAttribute);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {

        // var typeDeclarations = context.SyntaxProvider.ForAttributeWithMetadataName(
        //     ParamSerializableAttributeFullName,
        //     predicate: static (node, token) => node is ClassDeclarationSyntax or
        //         StructDeclarationSyntax or
        //         RecordDeclarationSyntax or
        //         InterfaceDeclarationSyntax,
        //     transform: static (context, token) =>
        //         (TypeDeclarationSyntax)context.TargetNode
        // );
        throw new System.NotImplementedException();
    }
}