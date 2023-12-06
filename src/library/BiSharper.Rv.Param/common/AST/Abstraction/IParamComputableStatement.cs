namespace BiSharper.Rv.Param.AST.Abstraction;

public interface IParamComputableStatement : IParamStatement
{
    public IParamStatement? ComputeOnContext(ParamContext context, ParamComputeOption option);
}