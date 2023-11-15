using BiSharper.Rv.Material.Model.Type;

namespace BiSharper.Rv.Material.Model;

public readonly struct MaterialDescription
{
    public static readonly RvNormalMaterialType DefaultMaterialType = new();
    public string Name { get; init; }
    public IRvMaterialType MaterialType { get; init; } = DefaultMaterialType;

    public MaterialDescription(string name, IRvMaterialType? materialType = null)
    {
        Name = name;
        MaterialType = materialType ?? DefaultMaterialType;
    }
}