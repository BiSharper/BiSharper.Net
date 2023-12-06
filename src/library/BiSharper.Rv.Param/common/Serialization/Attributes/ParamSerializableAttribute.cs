namespace BiSharper.Rv.Param.Serialization.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct)]
public class ParamSerializableAttribute : Attribute
{
    public ParamSerializationMode Mode { get; }

    public ParamSerializableAttribute(ParamSerializationMode mode = ParamSerializationMode.Class)
    {
        Mode = mode;
    }

}