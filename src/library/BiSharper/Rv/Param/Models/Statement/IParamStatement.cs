namespace BiSharper.Rv.Param.Models.Statement;

public interface IParamStatement
{
    public IParamContextHolder ParentContextHolder { get; }

    public ParamContext? ParamContext => ParentContextHolder as ParamContext;

}