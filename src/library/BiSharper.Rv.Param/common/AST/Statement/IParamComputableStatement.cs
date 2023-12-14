using BiSharper.Rv.Param.AST.Abstraction;

namespace BiSharper.Rv.Param.AST.Statement;

public interface IParamComputableStatement : IParamStatement
{
    public IParamStatement? ComputeOnContext(ParamContext context, ParamComputeOption option);
}