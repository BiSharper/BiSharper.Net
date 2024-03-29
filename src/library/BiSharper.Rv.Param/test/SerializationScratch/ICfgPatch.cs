﻿using BiSharper.Rv.Param.AST.Value;
using BiSharper.Rv.Param.AST.Value.Enumerable;
using BiSharper.Rv.Param.AST.Value.Numeric;
using BiSharper.Rv.Param.Serialization.Attributes;

namespace SerializationScratch;

[ParamSerializable]
public interface ICfgPatch
{
    [ParamProperty("units")]
    public IParamArray<ParamString> Units { get; }

    [ParamProperty("weapons")]
    public IParamArray<ParamString> Weapons { get; }

    [ParamProperty("requiredVersion")]
    public ParamFloat RequiredVersion { get; }

    [ParamProperty("requiredAddons")]
    public IParamArray<ParamString> RequiredAddons { get; }
}