using System.Collections.Immutable;
using BiSharper.Rv.Param.Serialization;

namespace BiSharper.Rv.Param.Generator.Metadata;

internal record StudGenerationMeta(
    ParamSerializationMode SerializationMode,
    ImmutableList<PropertyGenerationMeta> DefinedProperties,
    ImmutableList<ImplementationGenerationMeta> Implementations
);