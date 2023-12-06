using BiSharper.Rv.Param.Common.AST.Statement;

namespace BiSharper.Rv.Param.Common.AST.Abstraction;

public interface IParamStatement : IParamElement
{
    public ParamContext? ParentContext { get; }
}

public static class ParamStatementExtensions
{
    public static ParamContext? ParentContext(this IParamStatement statement) =>
        statement.Parent as ParamContext;

    public static ParamClass? ParentClass(this IParamStatement statement) =>
        statement.Parent as ParamClass;
}
