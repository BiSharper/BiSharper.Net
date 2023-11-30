using BiSharper.Rv.Render.Texture.Models;
using BiSharper.Rv.Sound.Ogg;

namespace BiSharper.Rv.Render.Texture.Source;

public interface IRvTextureSource
{
    public Dictionary<RvPacMipmap, int> Mipmaps { get; }
    public bool IgnoreLoadedLevels { get; }
    public OggDecoder? SoundDecoder { get; }
    public string RenderToTextureName => "InValid";
    public float RenderToTextureAspect => -1.0f;
    public bool LittleEndian => true;
    public RvColor AverageColor { get; }
    public RvColor MaxColor { get; }
    public bool IsAlpha { get; }
    public bool IsTransparentAlpha { get; }
    public bool Transparent { get; }
    public RvTextureType TextureType { get; }
    public RvPacFormat TextureFormat { get; }
    public RvTextureFilter MaximumTextureFilter => RvTextureFilter.Anisotropic16;

}