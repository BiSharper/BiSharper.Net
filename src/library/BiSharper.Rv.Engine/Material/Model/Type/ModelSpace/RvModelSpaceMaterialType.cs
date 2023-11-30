using System.Numerics;

namespace BiSharper.Rv.Render.Material.Model.Type.ModelSpace;

public record RvModelSpaceMaterialType(Vector3 Offset, IRvMaterialType BaseType) : RvDerivedMaterialType(BaseType)
{
    public Vector3 Offset { get; private init; } = Offset;
    private const string NormalName = "Model Space", NormalDescription = "Model Space type; Transform material to model space.";
    public override string DebugName => NormalName;
    public override string DebugDescription => NormalDescription;

    public override void Transform(RvMaterial material)
    {
        base.Transform(material);
        // for (int i=0; i<mat->_nTexGen; i++)
        // {
        //     TexGenInfo &stage = mat->_texGen[i];
        //     if (stage._uvSource==UVWorldPos)
        //     {
        //         // convert the matrix
        //         // modelToWorld is translation about _offset
        //         Matrix4 modelToWorld(MTranslation,_offset);
        //         // uv = worldToUv * modelToWorld * modelXYZ
        //         stage._uvTransform = stage._uvTransform*modelToWorld;
        //
        //         // change it to reflect changed space
        //         stage._uvSource = UVPos;
        //     }
        // }
    }

    public override bool Equals(IRvMaterialType? other) => base.Equals(other) && (other is not RvModelSpaceMaterialType modelSpace || Equals(modelSpace));
    
    public virtual bool Equals(RvModelSpaceMaterialType? other) => true;

    public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Offset);
}