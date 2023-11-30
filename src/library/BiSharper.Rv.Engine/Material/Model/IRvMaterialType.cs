using BiSharper.Rv.Render.Material.Model.Type;

namespace BiSharper.Rv.Render.Material.Model;

public interface IRvMaterialType : IRvMaterialTransformer, IEquatable<IRvMaterialType>
{
    public static readonly RvNormalMaterialType Default = new();

    public string DebugName { get; }
    public string DebugDescription { get; }
}