namespace BiSharper.Rv.Param.Serialization.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class ParamPropertyAttribute : Attribute
{
    public ParamPropertyAttribute(string name, bool required = false)
    {
        Name = name;
    }

    public string Name { get; }
}