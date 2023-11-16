namespace BiSharper.Rv.Material.Model.Type;

public sealed record RvRoadMaterialType(IRvMaterialType BaseType) : RvDerivedMaterialType(BaseType)
{
    //TODO: Describe and complete transformations
    private const string RoadTerrainName = "Road",
        RoadTerrainDescription =
            "RoadType: Material for road rendering. Shares many properties with terrain, but pixel shader is much simpler.";

    public override string DebugName => RoadTerrainName;
    public override string DebugDescription => RoadTerrainDescription;


    public override void Transform(RvMaterial material)
    {
  //       // road material is always using:
  // // PSRoad
  // // VSTerrain
  // // we assume artists design it as PSNormalMapDetailSpecularDIMap
  // mat->Load();
  // PixelShaderID ps = mat->GetPixelShaderID();
  // switch (ps)
  // {
  //   case PSAlphaShadow: case PSAlphaNoShadow:
  //     mat->SetPixelShaderID(PSRoad2Pass);
  //     mat->SetVertexShaderID(VSTerrain);
  //     return;
  //   case PSNormalMapDetailSpecularMap:
  //     // no additional work needed
  //     break;
  //   case PSNormal:
  //     // some additional textures are needed to act as the PSRoad
  //     // add detail texture to stage 1
  //     mat->_stage[1]._tex = GlobLoadTexture("#(argb,1,1,1)color(0.5,0.5,1,1)"); // normal map
  //     mat->_stage[1]._filter = TFTrilinear;
  //     mat->_stage[1]._texGen = 1;
  //     mat->_texGen[1]._uvSource = UVTex;
  //     mat->_texGen[1]._uvTransform = MIdentity;
  //   // fall-through to PSNormalMap
  //   case PSNormalMap:
  //     // some additional textures are needed to act as the PSRoad
  //     // add detail texture to stage 2
  //     mat->_stage[2]._tex = GlobLoadTexture("#(argb,1,1,1)color(0.5,0.5,0.5,1)"); // detail
  //     mat->_stage[2]._filter = TFTrilinear;
  //     mat->_stage[2]._texGen = 2;
  //     mat->_texGen[2]._uvSource = UVTex;
  //     mat->_texGen[2]._uvTransform = MIdentity;
  //     // add specular map to stage 3
  //     mat->_stage[3]._tex = GlobLoadTexture("#(argb,1,1,1)color(1,1,1,1)"); // Specular Map (dif, spec, power)
  //     mat->_stage[2]._filter = TFTrilinear;
  //     mat->_stage[3]._texGen = 3;
  //     mat->_texGen[3]._uvSource = UVTex;
  //     mat->_texGen[3]._uvTransform = MIdentity;
  //     mat->_stageCount = 3;
  //     mat->_nTexGen = 4;
  //     break;
  //   case PSNormalMapSpecularMap:
  //     // move specular map to stage 3
  //     Assert(mat->_stageCount+1==3);
  //     Assert(mat->_nTexGen==3);
  //     mat->_stage[3] = mat->_stage[2];
  //     mat->_stage[3]._texGen = 3;
  //     mat->_texGen[3] = mat->_texGen[2];
  //
  //     // add detail texture to stage 2
  //     mat->_stage[2]._tex = GlobLoadTexture("#(argb,1,1,1)color(0.5,0.5,0.5,1)"); // detail
  //     mat->_stage[2]._filter = TFTrilinear;
  //     mat->_stage[2]._texGen = 2;
  //     mat->_texGen[2]._uvSource = UVTex;
  //     mat->_texGen[2]._uvTransform = MIdentity;
  //     
  //     mat->_stageCount = 3;
  //     mat->_nTexGen = 4;
  //     break;
  //   default:
  //     RptF(
  //       "Warning: Pixel shader %s not supported for roads.",cc_cast(FindEnumName(ps))
  //     );
  //     break;
  // }
  // mat->SetPixelShaderID(PSRoad);
  // mat->SetVertexShaderID(VSTerrain);
  // //mat->SetVertexShaderID(VSNormalMap);
    }
}