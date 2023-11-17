using BiSharper.Rv.Param.Models.Value;

namespace BiSharper.Rv.Param.Models.Statement;

public interface IParamContext
{
    public string ContextName { get; }
    public IReadOnlyCollection<IParamStatement> Statements { get; }
    public IReadOnlyDictionary<ParamParMeta, IParamValue> Parameters { get; }
    public IReadOnlyDictionary<string, ParamContext> Contexts { get; }

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