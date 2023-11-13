using System.Numerics;
using BiSharper.Common.Math;
using BiSharper.Rv.Shape.Flags;

namespace BiSharper.Rv.Shape;

public class LODShape
{
    private ShapeRemarks _remarks = 0;
    private float _mass = 0;
    private List<float> _massArray = new List<float>();
    private float _inverseMass;
    private DetailLevel[] _levelsOfDetail; 
    private Vector3 _centerOfMass;
    private BTripointMatrix3 _inverseInertia;
    private BTripointMatrix3 _inertia;


    public LODShape(BinaryReader reader, string name, bool reversed)
    {
        var start = reader.BaseStream.Position;
        if (reversed) _remarks |= ShapeRemarks.Reversed;
        
        int lodCount = 1, version = 0;
        var tagged = false;
        _inverseMass = 1e10f;
        _inverseInertia = new BTripointMatrix3();
        _inverseInertia = new BTripointMatrix3();
        _centerOfMass = new Vector3();

        var signature = reader.ReadBytes(4);
        if (signature == "MLOD"u8)
        {
            tagged = true;
            version = reader.ReadInt32();
            lodCount = reader.ReadInt32();
        } else if (signature == "NLOD"u8)
        {
            tagged = true;
            lodCount = reader.ReadInt32();
        } else if (signature == "ODOL"u8)
        {
            //TODO
        }

        int versionMajor = version >> 8, versionMinor = version & 0xff;
        if (versionMajor > 1)
        {
            throw new NotSupportedException($"Unsupported Shape Version {versionMajor}.{versionMinor}");
        }

        bool treeCrownNeeded = false, canBlend = false;
        _levelsOfDetail = new DetailLevel[lodCount];
        for (var i = 0; i < lodCount; i++)
        {
            float resolution = 0;
            var shapeStart = reader.BaseStream.Position;
            bool wasMassArray = _massArray.Count > 0;
            
        }

    }
}