namespace BiSharper.Rv.Param.AST.Abstraction;

[Flags]
public enum ParamContextAccessibility : byte
{
    Default, //Default - Read Write
    ReadCreate, //Only allow adding new class members
    ReadOnly,
    ReadOnlyVerified, //Apply CRC Test

    DeletionProhibited = ReadCreate | CompleteImmutability,
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