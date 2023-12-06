using BiSharper.Rv.Param.Common.AST.Abstraction;

namespace BiSharper.Rv.Param.Common.AST.Statement;

public readonly struct ParamMutationStatement : IParamStatement
{
    public ParamContext ParentContext { get; }
    public bool Additive { get; }
    public string ParameterName { get; }
    public IParamValue ParameterValue { get; }
    public IParamElement Parent => (IParamElement) ParentContext;

    public ParamMutationStatement(string parameterName, IParamValue value, bool additive, ParamContext parent)
    {
        ParentContext = parent;
        Additive = additive;
        ParameterName = parameterName;
        ParameterValue = value;
    }
}