using BiSharper.Rv.Param.AST.Value;
using BiSharper.Rv.Param.AST.Value.Enumerable;
using BiSharper.Rv.Param.AST.Value.Numeric;

namespace SerializationScratch;

public readonly partial record struct CfgPatch
{
    public IParamArray<ParamString> Units { get; }
    public IParamArray<ParamString> Weapons { get; }
    public ParamFloat RequiredVersion { get; }
    public IParamArray<ParamString> RequiredAddons { get; }
}