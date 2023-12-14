using BiSharper.Rv.Param.AST.Abstraction;
using BiSharper.Rv.Param.AST.Statement;

namespace SerializationScratch;

public partial struct CfgPatch : IParamContext
{
    public ParamContextAccessibility Accessibility { get; }
    public string ContextName { get; }
    public bool NeedsFurtherContext { get; }

    public bool HasStatement(IParamStatement statement)
    {
        throw new NotImplementedException();
    }

    public bool TryGetClass(string name, out IParamContext? @class)
    {
        throw new NotImplementedException();
    }

    public bool TryGetParameter(string name, out ParamParameter? param)
    {
        throw new NotImplementedException();
    }

    public IParamContext? AddClass(IParamContext? context, ParamComputeOption mergeOption)
    {
        throw new NotImplementedException();
    }

    public IParamStatement? AddStatement(IParamStatement? statement, ParamComputeOption mergeOption)
    {
        throw new NotImplementedException();
    }

    public IParamContext? RemoveClass(string className)
    {
        throw new NotImplementedException();
    }

    public IParamContext MergeWith(IParamContext context, bool strict)
    {
        throw new NotImplementedException();
    }

    public ParamParameter? AddParameter(ParamParameter? parameter)
    {
        throw new NotImplementedException();
    }

    public ParamExternalContext? FindExternalContext(string name)
    {
        throw new NotImplementedException();
    }
}