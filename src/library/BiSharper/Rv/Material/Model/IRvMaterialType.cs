using BiSharper.Rv.Material.Model.Type;

namespace BiSharper.Rv.Material.Model;

public interface IRvMaterialType : IRvMaterialTransformer, IEquatable<IRvMaterialType>
{
    public static readonly RvNormalMaterialType Default = new();

    public string DebugName { get; }
    public string DebugDescription { get; }
}