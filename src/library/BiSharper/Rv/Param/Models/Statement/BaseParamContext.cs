using BiSharper.Rv.Param.Models.Value;

namespace BiSharper.Rv.Param.Models.Statement;

public abstract class BaseParamContext(string name) : IParamContext
{
    public IReadOnlyDictionary<ParamParMeta, IParamValue> Parameters
    {
        get => MutableParameters.AsReadOnly();
        init => MutableParameters = value.ToDictionary(k => k.Key, v => v.Value);
    }
    protected readonly Dictionary<ParamParMeta, IParamValue> MutableParameters = new();
    public IReadOnlyDictionary<string, ParamContext> Contexts
    {
        get => MutableContexts.AsReadOnly();
        init => MutableContexts = value.ToDictionary( k => k.Key, v => v.Value);
    }

    public virtual ParamContext? FindContext(string contextName, string? contextParent = null) =>
        Contexts
            .Where(x => x.Key == contextName)
            .Select(x => x.Value)
            .FirstOrDefault();

    protected readonly Dictionary<string, ParamContext> MutableContexts = new();

    public string ContextName { get; init; } = name;

    public IEnumerable<IParamStatement> Statements
    {
        get => MutableStatements.AsReadOnly();
        init => MutableStatements = value.ToList();
    }
    protected readonly List<IParamStatement> MutableStatements = new();


    public void AssignParameter(ParamParMeta meta, IParamValue? value, bool checkValidity = true)
    {
        if (value is null)
        {
            MutableParameters.Remove(meta);
            return;
        }

        MutableParameters[meta] = checkValidity ? meta.AssertValid(value) : value;
    }

    public void AssignContext(string key, ParamContext? context)
    {
        if (context is null)
        {
            MutableContexts.Remove(key);
            return;
        }

        MutableContexts[key] = context;
    }

    public void AddStatement(IParamStatement? statement)
    {
        if(statement is null || MutableStatements.Contains(statement)) return;
        MutableStatements.Add(statement);
    }
}