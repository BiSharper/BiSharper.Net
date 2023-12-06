namespace BiSharper.Rv.Param.Common.AST.Abstraction;

public interface IParamComputableStatement : IParamStatement
{
    public IParamStatement? ComputeOnContext(ParamContext context, ParamComputeOption option);
}