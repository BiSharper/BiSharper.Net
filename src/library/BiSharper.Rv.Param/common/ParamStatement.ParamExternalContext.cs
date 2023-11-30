namespace BiSharper.Rv.Param.Common;

public struct ParamExternalContext(string name, ParamContext context): IParamStatement
{
    public string ContextName { get; } = name;
    public ParamContext ParentContext { get; set;  } = context;
}