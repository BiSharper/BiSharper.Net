namespace BiSharper.Rv.Param.Serialization.Attributes;

[AttributeUsage( AttributeTargets.Interface)]
public class ParamSerializableAttribute : Attribute
{
    public ParamSerializationMode Mode { get; }
    public ParamGenerationMode GenerationMode { get; }

    public ParamSerializableAttribute(
        ParamSerializationMode mode = ParamSerializationMode.ClassGeneration,
        ParamGenerationMode generationMode = Serialization.ParamGenerationMode.ReadOnly | Serialization.ParamGenerationMode.ReadWrite)
    {
        Mode = mode;
        GenerationMode = generationMode;
    }

}