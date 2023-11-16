using System.Numerics;

namespace BiSharper.Rv.Material.Model.Type.ModelSpace;

public record RvSimpleTerrainMaterialType(Vector3 Offset, IRvMaterialType BaseType) : RvModelSpaceMaterialType(Offset, BaseType)
{
    private const string SimpleTerrainName = "Simple Terrain", SimpleTerrainDescription = "Simple Terrain Type: Model-Space material for distance terrain rendering; No specific transformations will be done.";
    public override string DebugName => SimpleTerrainName;
    public override string DebugDescription => SimpleTerrainDescription;

    public override void Transform(RvMaterial material)
    {
        base.Transform(material);
// #if TEXMAT_RELOAD
//     // note: reloading this type of material is currently not possible
//     // if it would be done, it would break
//     mat->_timestamp = INT_MAX;
// #endif
    }

    public override bool Equals(IRvMaterialType? other) => other is not RvGrassTerrainMaterialType? || base.Equals(other);
}