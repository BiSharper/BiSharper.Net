using System.Numerics;

namespace BiSharper.Rv.Render.Material.Model.Type.ModelSpace;

public sealed record RvGrassTerrainMaterialType(Vector3 Offset, IRvMaterialType BaseType) : RvSimpleTerrainMaterialType(Offset, BaseType)
{
    //TODO: Describe and complete transformations
    private const string GrassTerrainName = "Grass Terrain", GrassTerrainDescription = "Grass Terrain Type: Model-Space material for terrain grass rendering; No specific transformations will be done.";
    public override string DebugName => GrassTerrainName;
    public override string DebugDescription => GrassTerrainDescription;

    public override bool Equals(IRvMaterialType? other) => other is not RvGrassTerrainMaterialType || base.Equals(other);

    public override void Transform(RvMaterial material)
    {
        base.Transform(material);
        //
        // PixelShaderID ps = mat->GetPixelShaderID();
        // if (ps==PSTerrainX || ps==PSTerrainSimpleX || ps==PSTerrainSNX)
        // {
        //     mat->SetPixelShaderID(PSTerrainGrassX);
        // }
        // else if (ps>=PSTerrain1 && ps<=PSTerrain15)
        // {
        //     mat->SetPixelShaderID(PixelShaderID(ps+PSTerrainGrass1-PSTerrain1));
        // }
        // else if (ps>=PSTerrainSimple1 && ps<=PSTerrainSimple15)
        // {
        //     // note: I am not quite sure if Simple material can be converted
        //     // but it seems to me they can - they are multipass as well
        //     // only skipping some normal map computations
        //     mat->SetPixelShaderID(PixelShaderID(ps+PSTerrainGrass1-PSTerrainSimple1));
        // }
        // else
        // {
        //     Fail("Unknown terrain pixel shader during conversion to grass");
        // }
        // if (mat->GetVertexShaderID()==VSTerrain)
        // {
        //     mat->SetVertexShaderID(VSTerrainGrass);
        // }
        // else
        // {
        //     Fail("Unknown terrain vertex shader during conversion to grass");
        // }
    }
}