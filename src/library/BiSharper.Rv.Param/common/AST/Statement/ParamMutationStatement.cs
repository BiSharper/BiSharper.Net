using BiSharper.Rv.Param.AST.Abstraction;
using BiSharper.Rv.Param.AST.Value;

namespace BiSharper.Rv.Param.AST.Statement;

public readonly struct ParamMutationStatement : IParamComputableStatement
{
    public bool Additive { get; }
    public string ParameterName { get; }
    public ParamArray ParameterValue { get; }

    public ParamMutationStatement(string parameterName, ParamArray array, bool additive)
    {
        Additive = additive;
        ParameterName = parameterName;
        ParameterValue = array;
    }

    public IParamStatement ComputeOnContext(ParamContext context, ParamComputeOption option)
    {
        if (!context.TryGetParameter(ParameterName, out var parameter))
        {
            context.AddParameter(Additive
                ? new ParamParameter(ParameterName, ParameterValue, context)
                : new ParamParameter(ParameterName, new ParamArray(), context)
            );
        }

        if (parameter!.Value is not ParamArray array)
        {
            throw new Exception("Only arrays can be mutated!");
        }

        parameter.Value = new ParamArray(Additive
            ? array.Concat(ParameterValue)
            : array.Except(ParameterValue)
        );

        return parameter;
    }
}