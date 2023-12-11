using BiSharper.Rv.Param.AST.Statement;

namespace BiSharper.Rv.Param.AST.Abstraction;

public interface IParamContext : IParamContextualStatement
{
    ParamContextAccessibility Accessibility { get; }
    string ContextName { get; }
    IEnumerable<IParamStatement> Statements { get; }
    bool NeedsFurtherContext { get; }

    bool HasStatement(IParamStatement statement);
    bool TryGetClass(string name, out ParamContext? @class);
    bool TryGetParameter(string name, out ParamParameter? param);
    IParamStatement? AddStatement(IParamStatement? statement, ParamComputeOption mergeOption);
    ParamContext? RemoveClass(string className);
    ParamContext MergeWith(ParamContext context, bool strict);
    void AddClasses(ICollection<ParamClass?>? classes, ParamComputeOption computeOption);
    void AddStatements(IEnumerable<IParamStatement?>? statements, ParamComputeOption computeOption);
    void AddParameters(IEnumerable<ParamParameter?>? parameters);
    ParamParameter? AddParameter(ParamParameter? parameter);
    ParamExternalContext? FindExternalContext(string name);
}