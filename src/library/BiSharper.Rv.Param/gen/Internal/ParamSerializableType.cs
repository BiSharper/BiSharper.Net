using System;
using BiSharper.Rv.Param.Serialization;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BiSharper.Rv.Param.Generator.Internal;

internal struct ParamSerializableType
{
    public INamedTypeSymbol Symbol { get; }
    public ParamSerializationMode SerializationMode { get; }

    public ParamSerializableType(ISymbol typeSymbol, ReferenceSymbols reference)
    {
    }


    public bool ValidateForGeneration(TypeDeclarationSyntax syntax, out Diagnostic? diagnostic)
    {

        throw new NotImplementedException();
    }
}