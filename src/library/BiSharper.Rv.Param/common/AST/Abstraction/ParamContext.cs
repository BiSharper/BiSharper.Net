using System.Collections.Concurrent;
using System.Data;
using BiSharper.Rv.Param.AST.Statement;

namespace BiSharper.Rv.Param.AST.Abstraction;


public abstract class ParamContext : IParamContext
{
    private readonly ConcurrentDictionary<string, ParamParameter> _parameters = new();
    private readonly ConcurrentDictionary<string, ParamContext> _classes = new();
    private readonly List<IParamComputableStatement> _computableStatements = [];
    public ParamContextAccessibility Accessibility { get; protected set; }
    public string ContextName { get; protected set; }
    public IEnumerable<IParamStatement> Statements =>
        _computableStatements
            .Concat(_parameters.Values.Cast<IParamStatement>())
            .Concat(_classes.Values)
            .AsParallel();
    public bool NeedsFurtherContext => _computableStatements.Any();

    protected ParamContext(
        string name,
        ParamContextAccessibility accessibility = ParamContextAccessibility.Default,
        IEnumerable<IParamStatement>? statements = null
    )
    {
        ContextName = name;
        Accessibility = accessibility;
        AddStatements(statements, ParamComputeOption.Syntax);
    }

    public bool HasStatement(IParamStatement statement) => statement switch
    {
        ParamClass @class => _classes.ContainsKey(@class.ContextName),
        ParamParameter param => _parameters.ContainsKey(param.Name),
        _ => _computableStatements.Contains(statement)
    };

    public bool TryGetClass(string name, out ParamContext? @class) => _classes.TryGetValue(name, out @class);

    public bool TryGetParameter(string name, out ParamParameter? param) => _parameters.TryGetValue(name, out param);

    public IParamStatement? AddStatement(IParamStatement? statement, ParamComputeOption mergeOption)
    {
        if(statement is null || _computableStatements.Contains(statement)) return null;
        if (statement is not IParamComputableStatement computable) return statement switch
        {
            ParamClass classStatement => AddClass(classStatement, mergeOption) as ParamClass,
            ParamParameter parameterStatement => AddParameter(parameterStatement),
            _ => throw new Exception($"Unknown statement type \"{statement.GetType().FullName}\".")
        };
        if (Accessibility is not ParamContextAccessibility.Default
            && statement is ParamDeleteStatement
            && (Accessibility & ParamContextAccessibility.DeletionProhibited) != ParamContextAccessibility.Default
        ) throw new AccessViolationException("This context does not allow the deletion of class members.");
        if (mergeOption.ShouldCompute()) return computable.ComputeOnContext(this, mergeOption);
        _computableStatements.Add(computable);
        return computable;
    }

    public ParamContext? RemoveClass(string className)
    {
        if (!_classes.TryGetValue(className, out var clazz) && !_classes.TryRemove(className, out clazz))
            throw new IOException($"Failed to remove context under the name {className}.");
        return clazz;
    }

    public ParamContext MergeWith(ParamContext context, bool strict)
    {
        AddStatements(context.Statements, strict ? ParamComputeOption.ComputeStrict : ParamComputeOption.Compute);
        return this;
    }


    protected ParamContext? AddClass(ParamContext? context, ParamComputeOption mergeOption)
    {
        if (context is null) return null;
        if ((Accessibility & ParamContextAccessibility.CompleteImmutability) != ParamContextAccessibility.Default)
            throw new AccessViolationException("This context does not allow the adding of new class members.");
        var name = context.ContextName;
        if (FindExternalContext(name) is { } external && mergeOption.ShouldCompute()) _computableStatements.Remove(external);
        if (!_classes.TryGetValue(name, out var existing)) return _classes[name] = context;
        return mergeOption switch
        {
            ParamComputeOption.Syntax => throw new DuplicateNameException(
                $"Cannot add class \"{name}\" as a class under that name already exists."),
            ParamComputeOption.Compute => existing.MergeWith(context, false),
            ParamComputeOption.ComputeStrict => existing.MergeWith(context, true),
            _ => throw new ArgumentOutOfRangeException(nameof(mergeOption), mergeOption, null)
        };
    }

    public void AddClasses(ICollection<ParamClass?>? classes, ParamComputeOption computeOption)
    {
        if (classes == null) return;
        foreach (var @class in classes)
            if (@class != null) AddClass(@class, computeOption);
    }

    public void AddStatements(IEnumerable<IParamStatement?>? statements, ParamComputeOption computeOption)
    {
        if (statements == null) return;
        foreach (var statement in statements)
            if (statement != null) AddStatement(statement, computeOption);
    }


    public void AddParameters(IEnumerable<ParamParameter?>? parameters)
    {
        if (parameters == null) return;
        foreach (var parameter in parameters)
            if (parameter != null) AddParameter(parameter);
    }

    public ParamParameter? AddParameter(ParamParameter? parameter)
    {
        if (parameter is null) return null;
        if ((Accessibility & ParamContextAccessibility.ImmutableProperties) != ParamContextAccessibility.Default)
            throw new AccessViolationException("This context does not allow the editing of members.");

        var name = parameter.Name;

        return _parameters[name] = parameter;
    }

    public ParamExternalContext? FindExternalContext(string name)
    {
        foreach (var statement in _computableStatements)
        {
            if (statement is ParamExternalContext context && context.ContextName == name)
                return context;
        }

        return null;
    }

}

