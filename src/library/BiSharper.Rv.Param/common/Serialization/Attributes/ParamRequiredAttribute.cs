namespace BiSharper.Rv.Param.Serialization.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public sealed class ParamRequiredAttribute : Attribute
{
    //If soft and required element not found - stop reading without warning ( will be used for CfgPatches )
    public bool Soft { get; private init; }


    public ParamRequiredAttribute(bool soft)
    {
        Soft = soft;
    }
}