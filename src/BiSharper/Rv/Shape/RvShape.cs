using System.Numerics;
using BiSharper.Common.Math;
using BiSharper.Rv.Shape.Flags;

namespace BiSharper.Rv.Shape;

public class RvShape
{
    public List<float> MassArray { get; private set; }
    public DetailLevel[] DetailLevels { get; private set; }
    public Vector3 CenterOfMass { get; private set; }
    public float Mass { get; private set; }
    public float InverseMass { get; private set; }
    public BTripointMatrix3 Inertia { get; private set; }
    public BTripointMatrix3 InverseInertia { get; private set; }



    public RvShape(BinaryReader reader, string name)
    {
        var start = reader.BaseStream.Position;
        
        int lodCount = 1, shapeVersion = 0;
        var tagged = false;
        InverseMass = 1e10f;
        InverseInertia = new BTripointMatrix3();
        InverseInertia = new BTripointMatrix3();
        CenterOfMass = new Vector3();
        MassArray = new List<float>();

        var signature = reader.ReadBytes(4);
        if (signature == "MLOD"u8)
        {
            tagged = true;
            shapeVersion = reader.ReadInt32();
            lodCount = reader.ReadInt32();
        } else if (signature == "NLOD"u8)
        {
            tagged = true;
            lodCount = reader.ReadInt32();
        } else if (signature == "ODOL"u8)
        {
            //TODO
        }

        {
            int versionMajor = shapeVersion >> 8, versionMinor = shapeVersion & 0xff;
            if (versionMajor > 1)
            {
                throw new NotSupportedException($"Unsupported Shape Version {versionMajor}.{versionMinor}");
            }
        }

        bool treeCrownNeeded = false, canBlend = false;
        DetailLevels = new DetailLevel[lodCount];
        for (var i = 0; i < lodCount; i++)
        {
            var shapeStart = reader.BaseStream.Position;
            var wasMassArray = MassArray.Count > 0;
            var currentLod = DetailLevels[i] = new DetailLevel(reader, shapeVersion, MassArray, this);
            if (currentLod.Resolution < 0) throw new Exception($"Fatal: Lod {i} has a resolution less than 0.");
            
            //TODO geometry used
        }

    }


    public static GeometryUsed ResolveGeometryUsed(float resolution)
    {
        if (resolution <= 900)
        {
            return 0;
        }
        
        bool InSpec(float spec) => Math.Abs(resolution - spec) < spec * 1e-3f;

        throw new NotImplementedException();
    }
}