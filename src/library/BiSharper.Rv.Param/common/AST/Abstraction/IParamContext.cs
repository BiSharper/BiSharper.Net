using BiSharper.Rv.Param.AST.Statement;

namespace BiSharper.Rv.Param.AST.Abstraction;

public interface IParamContext : IParamStatement
{
    ParamContextAccessibility Accessibility { get; }
    string ContextName { get; }
    IEnumerable<IParamStatement> Statements { get; }
    bool NeedsFurtherContext { get; }

    bool HasStatement(IParamStatement statement);
    bool TryGetClass(string name, out IParamContext? @class);
    bool TryGetParameter(string name, out ParamParameter? param);
    IParamContext? AddClass(IParamContext? context, ParamComputeOption mergeOption);
    IParamStatement? AddStatement(IParamStatement? statement, ParamComputeOption mergeOption);
    IParamContext? RemoveClass(string className);
    IParamContext MergeWith(IParamContext context, bool strict);
    ParamParameter? AddParameter(ParamParameter? parameter);
    ParamExternalContext? FindExternalContext(string name);
}

public static class ParamContextExtensions
{
    public static void AddClasses(this IParamContext context, ICollection<IParamContext?>? classes, ParamComputeOption computeOption)
    {
        if (classes == null) return;
        foreach (var @class in classes)
            if (@class != null) context.AddClass(@class, computeOption);
    }

    public static void AddStatements(this IParamContext context, IEnumerable<IParamStatement?>? statements, ParamComputeOption computeOption)
    {
        if (statements == null) return;
        foreach (var statement in statements)
            if (statement != null) context.AddStatement(statement, computeOption);
    }


    public static void AddParameters(this IParamContext context, IEnumerable<ParamParameter?>? parameters)
    {
        if (parameters == null) return;
        foreach (var parameter in parameters)
            if (parameter != null) context.AddParameter(parameter);
    }
}