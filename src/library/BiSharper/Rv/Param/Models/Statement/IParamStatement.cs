namespace BiSharper.Rv.Param.Models.Statement;

public interface IParamStatement
{
    public IParamContext ParentContext { get; }

    public ParamContext? ParamContext => ParentContext as ParamContext;

}