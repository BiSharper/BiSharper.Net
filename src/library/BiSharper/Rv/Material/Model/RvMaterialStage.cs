using BiSharper.Rv.Texture;
using BiSharper.Rv.Texture.Models;

namespace BiSharper.Rv.Material.Model;

public struct RvMaterialStage(
    RvTexture texture,
    RvTextureFilter textureFilter,
    RvMaterial material)
{
    public readonly RvMaterial Parent = material;
    public RvTexture Texture = texture; 
    public RvTextureFilter TextureFilter { get; init; } = textureFilter;
}