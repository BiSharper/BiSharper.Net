namespace BiSharper.Rv.Render.Material.Model.Type;

public abstract record RvDerivedMaterialType(IRvMaterialType BaseType) : IRvMaterialType, IEquatable<RvDerivedMaterialType>
{
    public abstract string DebugName { get; }
    public abstract string DebugDescription { get; }
    public IRvMaterialType BaseType { get; private init; } = BaseType;

    public virtual void Transform(RvMaterial material)
    {
    }

    public virtual bool Equals(IRvMaterialType? other) => other is not RvDerivedMaterialType derived || Equals(derived);

    public virtual bool Equals(RvDerivedMaterialType? other) => other?.BaseType == BaseType;

    public override int GetHashCode() => BaseType.GetHashCode();
}