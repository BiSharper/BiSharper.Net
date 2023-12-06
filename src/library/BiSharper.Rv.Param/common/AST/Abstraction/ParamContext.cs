using System.Data;
using BiSharper.Rv.Param.Common.AST.Statement;

namespace BiSharper.Rv.Param.Common.AST.Abstraction;

[Flags]
public enum ParamContextAccessibility
{
    ReadWrite, //Default
    ReadCreate, //Only allow adding new class members
    ReadOnly,
    ReadOnlyVerified, //Apply CRC Test

    CompleteImmutability = ReadOnly | ReadOnlyVerified,
    ImmutableProperties = CompleteImmutability | ReadCreate,
}

[Flags]
public enum ParamComputeOption
{
    Syntax,
    Compute,
    ComputeStrict,
}

public static class ParamComputeOptionExtensions
{
    public static bool ShouldCompute(this ParamComputeOption option) =>
        option is ParamComputeOption.Compute or ParamComputeOption.ComputeStrict;
}

public abstract class ParamContext
{
    private readonly Dictionary<string, ParamParameter> _parameters = new();
    private readonly Dictionary<string, ParamClass> _classes = new();
    private readonly List<IParamStatement> _statements = new();
    public ParamContextAccessibility Accessibility { get; protected set; }
    public string ContextName { get; protected set; }
    public IEnumerable<IParamStatement> Statements => _statements.Concat(_parameters.Values).Concat(_classes.Values);

    protected ParamContext(
        string name,
        ParamContextAccessibility accessibility = ParamContextAccessibility.ReadWrite,
        IEnumerable<IParamStatement>? statements = null
    )
    {
        ContextName = name;
        Accessibility = accessibility;
        ComputeStatements(statements, ParamComputeOption.Syntax);
    }

    protected bool HasStatement(IParamStatement statement) => statement switch
    {
        ParamClass @class => _classes.ContainsKey(@class.ContextName),
        ParamParameter param => _parameters.ContainsKey(param.Name),
        _ => _statements.Contains(statement)
    };

    public bool TryGetClass(string name, out ParamClass? @class) => _classes.TryGetValue(name, out @class);

    public bool TryGetParameter(string name, out ParamParameter? param) => _parameters.TryGetValue(name, out param);


    protected void ComputeStatements(IEnumerable<IParamStatement?>? statements, ParamComputeOption mergeOption)
    {
        if (statements == null) return;
        foreach (var statement in statements)
        {
            if (statement is not null) ComputeStatement(statement, mergeOption);
        }
    }

    protected IParamStatement? ComputeStatement(IParamStatement? statement, ParamComputeOption mergeOption)
    {
        if(statement is null || _statements.Contains(statement)) return null;
        if (mergeOption.ShouldCompute())
        {
            switch (statement)
            {
                case ParamDeleteStatement delete: return ComputeDeleteStatement(delete, mergeOption);
                case ParamMutationStatement mutation: return ComputeMutationStatement(mutation, mergeOption);
            }
        }
        _statements.Add(statement);
        return statement;
    }

    protected ParamParameter? ComputeMutationStatement(ParamMutationStatement statement, ParamComputeOption mergeOption)
    {

        throw new NotImplementedException();
    }


    protected ParamClass? ComputeDeleteStatement(ParamDeleteStatement statement, ParamComputeOption mergeOption)
    {
        switch (mergeOption)
        {
            case ParamComputeOption.Syntax:
                _statements.Add(statement);
                return null;
            case ParamComputeOption.Compute:
                return RemoveClass(statement.ContextName);
            case ParamComputeOption.ComputeStrict:
                if (RemoveClass(statement.ContextName) is { } clazz) return clazz;
                throw new DuplicateNameException(
                    $"Cannot delete context named \"{statement.ContextName}\" as it does not exist!");
            default:
                throw new ArgumentOutOfRangeException(nameof(mergeOption), mergeOption, null);
        }
    }

    protected ParamClass? RemoveClass(string className)
    {
        if (_classes.TryGetValue(className, out var clazz))
        {
            _classes.Remove(className);
            return clazz;
        }

        return null;
    }

    protected void MergeWith(ParamContext context, bool strict)
    {
        AddParameters(context._parameters.Values);

    }

    protected ParamClass? AddClass(ParamClass? context, ParamComputeOption mergeOption)
    {
        if (context is null) return null;
        if ((Accessibility & ParamContextAccessibility.CompleteImmutability) != ParamContextAccessibility.ReadWrite)
            throw new AccessViolationException("This context does not allow the adding of new class members.");
        var name = context.ContextName;
        if (FindExternalContext(context.ContextName) is { } external && mergeOption.ShouldCompute()) _statements.Remove(external);
        if (_classes.TryGetValue(name, out var existing))
        {
            switch (mergeOption)
            {
                case ParamComputeOption.Syntax:
                    throw new DuplicateNameException($"Cannot add class \"{name}\" as a class under that name already exists.");
                case ParamComputeOption.Compute:
                    existing.MergeWith(context, false);
                    return existing;
                case ParamComputeOption.ComputeStrict:
                    existing.MergeWith(context, true);
                    return existing;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mergeOption), mergeOption, null);
            }
        }

        return _classes[name] = context;
    }

    protected void AddParameters(IEnumerable<ParamParameter?>? parameters)
    {
        if (parameters != null)
            foreach (var parameter in parameters)
                if (parameter != null) AddParameter(parameter);
    }

    protected ParamParameter? AddParameter(ParamParameter? parameter)
    {
        if (parameter is null) return null;
        if ((Accessibility & ParamContextAccessibility.ImmutableProperties) != ParamContextAccessibility.ReadWrite)
            throw new AccessViolationException("This context does not allow the editing of members.");

        var name = parameter.Name;

        return _parameters[name] = parameter;
    }

    public ParamExternalContext? FindExternalContext(string name)
    {
        foreach (var statement in _statements)
        {
            if (statement is ParamExternalContext context && context.ContextName == name)
                return context;
        }

        return null;
    }

}

