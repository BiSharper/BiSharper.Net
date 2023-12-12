using BiSharper.Rv.Common;

namespace BiSharper.Rv.Param.Serialization.Attributes;

public class ParamVersionSpecificAttribute : Attribute
{
    public RvEngineType EngineVersion { get; }

}