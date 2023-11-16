using BiSharper.Rv.Material.Model;
using BiSharper.Rv.Material.Model.Type;
using BiSharper.Rv.Param;
using BiSharper.Rv.Param.Models;

namespace BiSharper.Rv.Material;

public struct RvMaterial(string name, IRvMaterialType type)
{
    public string Name { get; set; } = name;
    public IRvMaterialType Type { get; init; } = type;
    private RvMaterialLod DetailLevel { get; init; }

    public RvMaterial(IParamContext context, string? name = null) : this(name ?? AssumedMaterialName(context), IRvMaterialType.Default)
    {
    }


    public static string AssumedMaterialName(IParamContext context)
    {
        if (context is not ParamRoot)
        {
            //Warn:: Name may be set incorrectly...
        }

        return context.ContextName;
    }
}