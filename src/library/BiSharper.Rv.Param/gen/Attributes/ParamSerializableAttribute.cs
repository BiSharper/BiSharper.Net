using System;

namespace BiSharper.Rv.Param.Generator.Attributes;

[AttributeUsage(
    AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface,
    Inherited = false
)]
public sealed class ParamSerializableAttribute : Attribute
{
    public ParamSerializationStrategy SerializationStrategy { get; }


    public ParamSerializableAttribute(ParamSerializationStrategy serializationStrategy = ParamSerializationStrategy.Context)
    {
        SerializationStrategy = serializationStrategy;
    }

}

[AttributeUsage(
    AttributeTargets.Field | AttributeTargets.Property
)]
public sealed class ParamIncludeAttribute : Attribute;

[AttributeUsage(
    AttributeTargets.Constructor
)]
public sealed class ParamConstructorAttribute : Attribute;