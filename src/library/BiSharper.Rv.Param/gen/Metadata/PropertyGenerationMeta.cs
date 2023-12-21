using BiSharper.Rv.Param.Generator.Internal;

namespace BiSharper.Rv.Param.Generator.Metadata;

internal readonly record struct PropertyGenerationMeta(
    StudGenerationMeta Stud,
    BaseParameterType ParamType,
    string ParamName,
    bool Required
);