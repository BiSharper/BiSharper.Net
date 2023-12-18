using BiSharper.Rv.Param.AST.Value;
using BiSharper.Rv.Param.AST.Value.Enumerable;
using BiSharper.Rv.Param.AST.Value.Numeric;
using BiSharper.Rv.Param.Serialization.Attributes;

namespace SerializationScratch;

[ParamSerializable]
public interface ICfgPatch
{
    [ParamMember("units")]
    public IParamArray<ParamString> Units { get; set; }

    [ParamMember("weapons")]
    public IParamArray<ParamString> Weapons { get; set; }

    [ParamMember("requiredVersion")]
    public ParamFloat RequiredVersion { get; set; }

    [ParamMember("requiredAddons")]
    public IParamArray<ParamString> RequiredAddons { get; set; }
}