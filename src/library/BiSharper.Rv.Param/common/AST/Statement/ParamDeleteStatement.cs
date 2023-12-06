using System.Data;
using BiSharper.Rv.Param.Common.AST.Abstraction;

namespace BiSharper.Rv.Param.Common.AST.Statement;

public readonly struct ParamDeleteStatement : IParamComputableStatement
{
    public string ContextName { get; }

    public ParamDeleteStatement(string target) => ContextName = target;

    public IParamStatement? ComputeOnContext(ParamContext context, ParamComputeOption option)
    {
        switch (option)
        {
            case ParamComputeOption.Syntax:
                return this;
            case ParamComputeOption.Compute:
                return context.RemoveClass(ContextName);
            case ParamComputeOption.ComputeStrict:
                if (context.RemoveClass(ContextName) is { } clazz) return clazz;
                throw new DuplicateNameException(
                    $"Cannot delete context named \"{ContextName}\" as it does not exist!");
            default:
                throw new ArgumentOutOfRangeException(nameof(option), option, null);
        }
    }
}