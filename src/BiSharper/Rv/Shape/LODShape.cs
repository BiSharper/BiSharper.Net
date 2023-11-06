using BiSharper.Common.Math;
using BiSharper.Rv.Shape.Flags;

namespace BiSharper.Rv.Shape;

public class LODShape
{

    private ShapeRemarks _remarks = 0;
    private float _mass = 0;
    private float _inverseMass;
    private BTripointMatrix3 _inverseInertia;
    private BTripointMatrix3 _inertia;


    public LODShape(BinaryReader reader, string name, bool reversed)
    {
        var start = reader.BaseStream.Position;
        if (reversed) _remarks |= ShapeRemarks.Reversed;
        
        int lodCount = 1, version = 0;
        bool tagged=false;
        _inverseMass = 1e10f;
        

    }
}