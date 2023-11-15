using System.Runtime.Serialization;

namespace BiSharper.Rv.Material.Model;

public interface IRvMaterialType : IRvMaterialTransformer, IEquatable<IRvMaterialType>
{
    public string DebugName { get; }
    public string DebugDescription { get; }
}