using BiSharper.Rv.Render.Texture;
using BiSharper.Rv.Render.Texture.Models;

namespace BiSharper.Rv.Render.Material.Model;

public struct RvMaterialStage(
    RvTexture texture,
    RvTextureFilter textureFilter,
    RvMaterial material)
{
    public readonly RvMaterial Parent = material;
    public RvTexture Texture = texture; 
    public RvTextureFilter TextureFilter { get; init; } = textureFilter;
}