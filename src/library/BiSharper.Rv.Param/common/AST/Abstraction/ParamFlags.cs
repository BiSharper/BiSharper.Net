namespace BiSharper.Rv.Param.Common.AST.Abstraction;

[Flags]
public enum ParamContextAccessibility
{
    ReadWrite, //Default
    ReadCreate, //Only allow adding new class members
    ReadOnly,
    ReadOnlyVerified, //Apply CRC Test

    CompleteImmutability = ReadOnly | ReadOnlyVerified,
    ImmutableProperties = CompleteImmutability | ReadCreate,
}

[Flags]
public enum ParamComputeOption
{
    Syntax,
    Compute,
    ComputeStrict,
}

public static class ParamComputeOptionExtensions
{
    public static bool ShouldCompute(this ParamComputeOption option) =>
        option is ParamComputeOption.Compute or ParamComputeOption.ComputeStrict;
}