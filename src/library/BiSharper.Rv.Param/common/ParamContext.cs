namespace BiSharper.Rv.Param.Common;

public enum ParamAccessMode
{
    ReadWrite, //Default
    ReadCreate, //Only allow adding new class members
    ReadOnly,
    ReadOnlyVerified //Apply CRC Test
}

public abstract class ParamContext(
    string name,
    IEnumerable<IParamStatement>? statements = null,
    IDictionary<ParamParMeta, IParamValue>? parameters = null,
    IDictionary<string, ParamClass>? classes = null
)
{
    public string ContextName { get; protected set; } = name;

    public IEnumerable<IParamStatement> Statements => MutableStatements.AsReadOnly();
    public IEnumerable<KeyValuePair<ParamParMeta, IParamValue>> Parameters => MutableParameters;
    public IEnumerable<KeyValuePair<string, ParamClass>> Classes => MutableClasses;

    protected List<IParamStatement> MutableStatements { get; } = new(statements ?? Enumerable.Empty<IParamStatement>());
    protected Dictionary<ParamParMeta, IParamValue> MutableParameters { get; }= parameters != null
        ? new Dictionary<ParamParMeta, IParamValue>(parameters) :
        new Dictionary<ParamParMeta, IParamValue>();

    protected readonly Dictionary<string, ParamClass> MutableClasses = classes != null
        ? new Dictionary<string, ParamClass>(classes)
        : new Dictionary<string, ParamClass>();

    public virtual ParamClass? FindContext(string contextName, string? contextParent = null) =>
        Classes
            .Where(x => x.Key == contextName)
            .Select(x => x.Value)
            .FirstOrDefault();

    public IParamValue? FindValue(ParamParMeta meta)
    {
        MutableParameters.TryGetValue(meta, out var value);
        return value;
    }

    public void AssignParameter(ParamParMeta meta, IParamValue? value, bool checkValidity = true)
    {
        if (value is null)
        {
            MutableParameters.Remove(meta);
            return;
        }

        MutableParameters[meta] = checkValidity ? meta.AssertValid(value) : value;
    }

    public void AssignContext(string key, ParamClass? context)
    {
        if (context is null)
        {
            MutableClasses.Remove(key);
            return;
        }

        MutableClasses[key] = context;
    }

    public void AddStatement(IParamStatement? statement)
    {
        if(statement is null || MutableStatements.Contains(statement)) return;
        MutableStatements.Add(statement);
    }

}

public static class BaseParamContextExtensions
{
    public static IEnumerable<KeyValuePair<ParamParMeta, T>> GetParameters<T>(this ParamContext context)
        where T : IParamValue =>
        context.Parameters
            .Where(p => p.Value is T)
            .Select(p => new KeyValuePair<ParamParMeta, T>(p.Key, (T)p.Value));

    public static IEnumerable<T> GetStatements<T>(this ParamContext context)
        where T : IParamStatement => context.Statements.OfType<T>();

    public static float? GetFloat(this ParamContext context, string name) =>
        (float?) GetValue(context, ParamValueType.Float, name);

    public static string? GetString(this ParamContext context, string name) =>
        (string?) GetValue(context, ParamValueType.String, name);

    public static long? GetLong(this ParamContext context, string name) =>
        (long?) GetValue(context, ParamValueType.Long, name);

    public static float? GetInteger(this ParamContext context, string name) =>
        (int?) GetValue(context, ParamValueType.Integer, name);

    public static ParamClass? GetClass(this ParamContext context, string name) =>
        context.FindContext(name);

    public static object? GetValue(this ParamContext context, ParamValueType type, string name) =>
        (from pair in context.Parameters
            let key = pair.Key where
                key.Name == name &&
                key.ValueType == type
            select pair.Value.ValueUnwrapped
        ).FirstOrDefault();

    public static ParamClass? FindOrAssignContext(this ParamContext context, string contextName, string? contextParent, ParamClass? value = null)
    {
        if (value != null)
            context.AssignContext(contextName, value);

        return context.FindContext(contextName, contextParent);
    }

    public static IParamValue? FindOrSetValue(this ParamContext context, string name, IParamValue? value = null)
    {
        var meta = (ParamParMeta?)context.Parameters.FirstOrDefault(m => m.Key.Name == context.ContextName).Key
                   ?? throw new Exception($"No parameter was found under the name {name}");

        if (value != null)
            context.AssignParameter(meta, value);

        return context.FindValue(meta);
    }

    public static IParamValue? FindOrSetValue(this ParamContext context, ParamParMeta meta, IParamValue? value = null)
    {
        if (value != null)
            context.AssignParameter(meta, value);

        return context.FindValue(meta);
    }
}