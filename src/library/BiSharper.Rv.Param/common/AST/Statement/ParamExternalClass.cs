using BiSharper.Rv.Param.Common.AST.Abstraction;

namespace BiSharper.Rv.Param.Common.AST.Statement;

public readonly struct ParamExternalContext : IParamComputableStatement
{
    public string ContextName { get; }

    public ParamExternalContext(string name) => ContextName = name;
    //TODO
    public IParamStatement ComputeOnContext(ParamContext context, ParamComputeOption option) => this;
}