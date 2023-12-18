using BiSharper.Rv.Param.AST.Abstraction;
using BiSharper.Rv.Param.AST.Value;
using BiSharper.Rv.Param.AST.Value.Enumerable;

namespace BiSharper.Rv.Param.AST.Statement;

public readonly struct ParamMutationStatement<T> : IParamComputableStatement where T : IParamValue
{
    public bool Additive { get; }
    public string ParameterName { get; }
    public ParamReadOnlyList<T> Parameter { get; }

    public ParamMutationStatement(string parameterName, ParamReadOnlyList<T> list, bool additive)
    {
        Additive = additive;
        ParameterName = parameterName;
        Parameter = list;
    }

    public IParamStatement ComputeOnContext(ParamContext context, ParamComputeOption option)
    {
        if (!context.TryGetParameter(ParameterName, out var parameter) )
        {
            context.AddParameter(parameter = Additive
                ? new ParamParameter(ParameterName, Parameter.ToParamList(), context)
                : new ParamParameter(ParameterName, new ParamList<T>(), context)
            );
            return parameter;
        }

        if (parameter?.Value is not ParamList<T> list)
        {
            throw new Exception("Only mutable arrays can be mutated!");
        }


        parameter.Value = new ParamList<T>(Additive
            ? list.Concat(Parameter)
            : list.Except(Parameter)
        );

        return parameter;
    }
}