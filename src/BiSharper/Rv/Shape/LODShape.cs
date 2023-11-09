using System.Numerics;
using BiSharper.Common.Math;
using BiSharper.Rv.Shape.Flags;

namespace BiSharper.Rv.Shape;

public class LODShape
{

    private ShapeRemarks _remarks = 0;
    private float _mass = 0;
    private float _inverseMass;
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
        


    }
}