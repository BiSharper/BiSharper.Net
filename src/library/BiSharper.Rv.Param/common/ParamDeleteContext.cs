namespace BiSharper.Rv.Param.Common;

public struct ParamDeleteContext(string target, ParamContext parent): IParamStatement
{
    public string ContextName { get; } = target;
    public ParamContext ParentContext { get; set; } = parent;
}