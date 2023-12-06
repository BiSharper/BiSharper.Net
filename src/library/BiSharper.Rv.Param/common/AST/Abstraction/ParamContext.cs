using System.Data;
using BiSharper.Rv.Param.Common.AST.Statement;
using BiSharper.Rv.Param.Common.AST.Value;

namespace BiSharper.Rv.Param.Common.AST.Abstraction;


public abstract class ParamContext
{
    private readonly Dictionary<string, ParamParameter> _parameters = new();
    private readonly Dictionary<string, ParamClass> _classes = new();
    private readonly List<IParamComputableStatement> _computableStatements = new();
    public ParamContextAccessibility Accessibility { get; protected set; }
    public string ContextName { get; protected set; }
    public IEnumerable<IParamStatement> Statements => _computableStatements.Concat(_parameters.Values.Cast<IParamStatement>()).Concat(_classes.Values);
    public bool NeedsFurtherContext => _computableStatements.Any();

    protected ParamContext(
        string name,
        ParamContextAccessibility accessibility = ParamContextAccessibility.ReadWrite,
        IEnumerable<IParamStatement>? statements = null
    )
    {
        ContextName = name;
        Accessibility = accessibility;
        AddStatements(statements, ParamComputeOption.Syntax);
    }

    protected bool HasStatement(IParamStatement statement) => statement switch
    {
        ParamClass @class => _classes.ContainsKey(@class.ContextName),
        ParamParameter param => _parameters.ContainsKey(param.Name),
        _ => _computableStatements.Contains(statement)
    };

    public bool TryGetClass(string name, out ParamClass? @class) => _classes.TryGetValue(name, out @class);

    public bool TryGetParameter(string name, out ParamParameter? param) => _parameters.TryGetValue(name, out param);

    protected IParamStatement? AddStatement(IParamStatement? statement, ParamComputeOption mergeOption)
    {
        if(statement is null || _computableStatements.Contains(statement)) return null;
        if (statement is IParamComputableStatement computable)
        {
            if (mergeOption.ShouldCompute()) return computable.ComputeOnContext(this, mergeOption);
            _computableStatements.Add(computable);
            return computable;
        }

        return statement switch
        {
            ParamClass classStatement => AddClass(classStatement, mergeOption),
            ParamParameter parameterStatement => AddParameter(parameterStatement),
            _ => throw new Exception($"Unknown statement type \"{statement.GetType().FullName}\".")
        };
    }

    public ParamClass? RemoveClass(string className)
    {
        if (_classes.TryGetValue(className, out var clazz))
        {
            _classes.Remove(className);
            return clazz;
        }

        return null;
    }

    protected void MergeWith(ParamContext context, bool strict) =>
        AddStatements(context.Statements, strict ? ParamComputeOption.ComputeStrict : ParamComputeOption.Compute);


    protected ParamClass? AddClass(ParamClass? context, ParamComputeOption mergeOption)
    {
        if (context is null) return null;
        if ((Accessibility & ParamContextAccessibility.CompleteImmutability) != ParamContextAccessibility.ReadWrite)
            throw new AccessViolationException("This context does not allow the adding of new class members.");
        var name = context.ContextName;
        if (FindExternalContext(context.ContextName) is { } external && mergeOption.ShouldCompute()) _computableStatements.Remove(external);
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

    protected void AddClasses(IEnumerable<ParamClass?>? classes, ParamComputeOption computeOption)
    {
        if (classes != null)
            foreach (var @class in classes)
                if (@class != null) AddClass(@class, computeOption);
    }

    protected void AddStatements(IEnumerable<IParamStatement?>? statements, ParamComputeOption computeOption)
    {
        if (statements != null)
            foreach (var statement in statements)
                if (statement != null) AddStatement(statement, computeOption);
    }


    protected void AddParameters(IEnumerable<ParamParameter?>? parameters)
    {
        if (parameters != null)
            foreach (var parameter in parameters)
                if (parameter != null) AddParameter(parameter);
    }

    public ParamParameter? AddParameter(ParamParameter? parameter)
    {
        if (parameter is null) return null;
        if ((Accessibility & ParamContextAccessibility.ImmutableProperties) != ParamContextAccessibility.ReadWrite)
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

