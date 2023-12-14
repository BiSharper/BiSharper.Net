using BiSharper.Rv.Common;
using BiSharper.Rv.Param.Serialization.Attributes;

namespace SerializationScratch;

[ParamSerializable]
public partial struct CfgPatch
{

    [ParamMember("units")]
    public partial IEnumerable<string> Units();

    [ParamMember("weapons")]
    public partial IEnumerable<string> Weapons();

    [ParamMember("requiredVersion")]
    public partial float RequiredVersion();

    [ParamMember("requiredAddons")]
    public partial IEnumerable<string> RequiredAddons();
}