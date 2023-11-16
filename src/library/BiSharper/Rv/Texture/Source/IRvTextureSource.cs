using BiSharper.Rv.Sound.Ogg;
using BiSharper.Rv.Texture.Models;

namespace BiSharper.Rv.Texture.Source;

public interface IRvTextureSource
{
    public bool IgnoreLoadedLevels { get; }
    public OggDecoder? SoundDecoder { get; }
    public string RenderToTextureName => "InValid";
    public float RenderToTextureAspect => -1.0f;
    public RvTextureFilter MaximumTextureFilter => RvTextureFilter.Anisotropic16;
    public bool IsLittleEndian { get; }
}