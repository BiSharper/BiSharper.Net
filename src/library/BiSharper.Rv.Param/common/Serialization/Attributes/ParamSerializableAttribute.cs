namespace BiSharper.Rv.Param.Serialization.Attributes;

[AttributeUsage( AttributeTargets.Interface)]
public class ParamSerializableAttribute : Attribute
{
    public ParamSerializationMode Mode { get; }

    public ParamSerializableAttribute(ParamSerializationMode mode = ParamSerializationMode.ClassGeneration)
    {
        Mode = mode;
    }
}