namespace BiSharper.Rv.Param.Serialization.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class ParamParameterNameAttribute : Attribute
{
    public ParamParameterNameAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
}