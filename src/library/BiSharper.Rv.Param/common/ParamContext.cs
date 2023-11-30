namespace BiSharper.Rv.Param.Common;

public enum ParamAccessMode
{
    ReadWrite, //Default
    ReadCreate, //Only allow adding new class members
    ReadOnly,
    ReadOnlyVerified //Apply CRC Test
}

public abstract class ParamContext
{
    private readonly Dictionary<string, ParamParameter> _parameters = new();
    private readonly Dictionary<string, ParamClass> _classes = new();
    private readonly List<IParamStatement> _statements;
    public string ContextName { get; protected set; }
    public IEnumerable<IParamStatement> Statements => _statements.AsReadOnly();
    public IEnumerable<KeyValuePair<string, ParamParameter>> Parameters => _parameters;
    public IEnumerable<KeyValuePair<string, ParamClass>> Classes => _classes;

    protected ParamContext(string name, IEnumerable<IParamStatement>? statements = null)
    {
        ContextName = name;
        _statements = new List<IParamStatement>(statements ?? Enumerable.Empty<IParamStatement>());
        foreach (var @class in this.GetStatements<ParamClass>()) AssignContext(string.Empty, @class);
        foreach (var parameter in this.GetStatements<ParamParameter>()) AssignParameter(parameter);
    }



    public virtual ParamClass? FindContext(string contextName, string? contextParent = null) =>
        Classes
            .Where(x => x.Key == contextName)
            .Select(x => x.Value)
            .FirstOrDefault();

    public ParamParameter? FindParameter(string name)
    {
        _parameters.TryGetValue(name, out var value);
        return value;
    }

    public IParamValue? FindValue(string name, out ParamOperatorType operatorType)
    {
        _parameters.TryGetValue(name, out var value);
        operatorType = value.Operator;
        return value.Value;
    }

    public IParamValue? GetValue(string name)
    {
        _parameters.TryGetValue(name, out var parameter);
        return parameter.Value;
    }

    protected void AssignParameter(ParamParameter parameter, bool checkValidity = true)
    {
        if (checkValidity && !parameter.Valid)
            throw new Exception($"Identifier {parameter.Name} is not valid or operator \"{parameter.Operator}\" is unsupported.");

        if (_statements.Contains(parameter)) _statements.Add(parameter);
        _parameters[parameter.Name] = parameter;
    }

    public void AssignParameter(string name, IParamValue? value, ParamOperatorType operatorType, bool checkValidity = true)
    {
        if (value is null)
        {
            if (!_parameters.TryGetValue(name, out var parameter)) return;

            _statements.Remove(parameter);
            _parameters.Remove(name);
            return;
        }

        AssignParameter(new ParamParameter(name, operatorType, value, this));
    }

    public void AssignContext(string key, ParamClass? context)
    {
        if (context is null)
        {
            if (!_classes.TryGetValue(key, out context)) return;
            _classes.Remove(key);
            _statements.Remove(context);
            return;
        }
        if (_statements.Contains(context)) _statements.Add(context);
        _classes[context.ContextName] = context;
    }

    public void AddStatement(IParamStatement? statement)
    {
        if(statement is null || _statements.Contains(statement)) return;
        switch (statement)
        {
            case ParamClass @class:
                AssignContext(string.Empty, @class);
                break;
            case ParamParameter parameter:
                AssignParameter(parameter);
                break;
        }
    }

}

public static class BaseParamContextExtensions
{
    public static IEnumerable<T> GetStatements<T>(this ParamContext context)
        where T : IParamStatement => context.Statements.OfType<T>();

    public static ParamFloat? GetFloat(this ParamContext context, string name)
    {
        if (context.GetValue(name) is not ParamFloat paramValue) return null;
        return paramValue;
    }

    public static string? GetString(this ParamContext context, string name)
    {
        if (context.GetValue(name) is not ParamString paramValue) return null;
        return paramValue;
    }

    public static long? GetLong(this ParamContext context, string name)
    {
        if (context.GetValue(name) is not ParamLong paramValue) return null;
        return paramValue;
    }
    public static int? GetInteger(this ParamContext context, string name)
    {
        if (context.GetValue(name) is not ParamInteger paramValue) return null;
        return paramValue;
    }

    public static ParamClass? GetClass(this ParamContext context, string name) =>
        context.FindContext(name);

    public static ParamClass? FindOrAssignContext(this ParamContext context, string contextName, string? contextParent, ParamClass? value = null)
    {
        if (value != null)
            context.AssignContext(contextName, value);

        return context.FindContext(contextName, contextParent);
    }
}