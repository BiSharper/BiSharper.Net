namespace BiSharper.Rv.Param.Serialization.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
public class ParamMemberAttribute : Attribute
{
    public ParamMemberAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
}