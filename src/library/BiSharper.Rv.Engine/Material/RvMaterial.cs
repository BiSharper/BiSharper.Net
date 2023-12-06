using BiSharper.Rv.Param;
using BiSharper.Rv.Param.Common;
using BiSharper.Rv.Param.Common.AST;
using BiSharper.Rv.Param.Common.AST.Abstraction;
using BiSharper.Rv.Render.Material.Model;

namespace BiSharper.Rv.Render.Material;

public struct RvMaterial(string name, IRvMaterialType type)
{
    public string Name { get; set; } = name;
    public IRvMaterialType Type { get; init; } = type;
    private RvMaterialLod DetailLevel { get; init; }

    public RvMaterial(ParamContext context, string? name = null) : this(name ?? AssumedMaterialName(context), IRvMaterialType.Default)
    {
    }


    public static string AssumedMaterialName(ParamContext context)
    {
        if (context is not ParamDocument)
        {
            //Warn:: Name may be set incorrectly...
        }

        return context.ContextName;
    }
}