
namespace BiSharper.Rv.Param.Models.Statement;

public class ParamContext(string name, string? super = null) : BaseParamContext(name)
{
    public required IParamContext ParentContext { get; init; }
    public string? ConformsTo { get; init; } = super;

    public override ParamContext? FindContext(string contextName, string? contextParent = null) =>
        Contexts
            .Where(x => x.Key == contextName)
            .Select(x => x.Value)
            .FirstOrDefault();
}