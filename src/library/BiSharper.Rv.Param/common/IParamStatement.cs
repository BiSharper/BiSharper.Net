namespace BiSharper.Rv.Param.Common;

public interface IParamStatement
{
    public ParamContext ParentContext { get; set; }
}

public static class ParamStatementExtensions
{
    public static ParamClass? GetParamContext(this IParamStatement statement) => statement as ParamClass;
}