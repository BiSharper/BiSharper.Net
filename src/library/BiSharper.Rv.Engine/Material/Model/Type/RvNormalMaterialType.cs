namespace BiSharper.Rv.Render.Material.Model.Type;

public record RvNormalMaterialType : IRvMaterialType
{
    private const string NormalName = "Normal", NormalDescription = "Normal type; No transformation takes place.";
    public virtual string DebugName => NormalName;
    public virtual string DebugDescription => NormalDescription;
    
    public virtual void Transform(RvMaterial material)
    {
    }

    public virtual bool Equals(IRvMaterialType? other) => other is not null;


}