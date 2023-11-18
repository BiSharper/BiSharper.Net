using BiSharper.Rv.Param.Models.Value;

namespace BiSharper.Rv.Param.Models.Statement;

public interface IParamContext
{
    public string ContextName { get; }
    public IEnumerable<IParamStatement> Statements { get; }
    public IReadOnlyDictionary<ParamParMeta, IParamValue> Parameters { get; }
    public IReadOnlyDictionary<string, ParamContext> Contexts { get; }

    public ParamContext? this[string contextName, string? contextParent]
    {
        get => FindContext(contextName, contextParent);
        set => AssignContext(contextName, value);
    }

    public ParamContext? FindContext(string contextName, string? contextParent);

    public IParamValue? this[string name]
    {
        get => this[
                (ParamParMeta?)Parameters.Keys.FirstOrDefault(meta => meta.Name == ContextName) ??
                throw new Exception($"No parameter was found under the name {name}")
        ];
        set => this[
            (ParamParMeta?)Parameters.Keys.FirstOrDefault(meta => meta.Name == ContextName) ??
            throw new Exception($"No parameter was found under the name {name}")
        ] = value;
    }

    public IParamValue? this[ParamParMeta meta]
    {
        get => Parameters.GetValueOrDefault(meta);
        set => AssignParameter(meta, value);
    }

    public void AssignParameter(ParamParMeta meta, IParamValue? value, bool checkValidity = true);
    public void AssignContext(string key, ParamContext? context);
    public void AddStatement(IParamStatement? statement);
}

public static class ParamContextExtensions
{
    public static IEnumerable<KeyValuePair<ParamParMeta, T>> GetParameters<T>(this IParamContext context)
        where T : IParamValue =>
        context.Parameters
            .Where(p => p.Value is T)
            .Select(p => new KeyValuePair<ParamParMeta, T>(p.Key, (T)p.Value));

    public static IEnumerable<T> GetStatements<T>(this IParamContext context)
        where T : IParamStatement => context.Statements.OfType<T>();
}