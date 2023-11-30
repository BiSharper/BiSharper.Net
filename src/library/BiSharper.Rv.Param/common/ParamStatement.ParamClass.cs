namespace BiSharper.Rv.Param.Common;

public sealed class ParamClass(string name, ParamContext parent, string? super = null) : ParamContext(name), IParamStatement
{
    public ParamContext ParentContext { get; set; } = parent;
    public string? ConformsTo { get; init; } = super;

    public override ParamClass? FindContext(string contextName, string? contextParent = null) =>
        Classes
            .Where(x => x.Key == contextName)
            .Select(x => x.Value)
            .FirstOrDefault();

}