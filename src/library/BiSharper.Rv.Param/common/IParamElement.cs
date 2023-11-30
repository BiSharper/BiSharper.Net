namespace BiSharper.Rv.Param.Common;

public interface IParamElement
{
    public ParamContext ParentContext { get; }
}

public static class ParamElementExtensions
{
    public static ParamClass? GetParamContext(this IParamElement element) => element as ParamClass;
}