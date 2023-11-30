using System.Collections;

namespace BiSharper.Rv.Render.Shape.Types.Poly;

public readonly struct PolygonVertices : IEnumerable<short>
{
    private readonly short[] _vertices = Array.Empty<short>();

    public PolygonVertices()
    {
    }

    public const int MaxPolygonCount = 32;

    public int Count => _vertices.Length;
    
    public short this[int index]
    {
        get
        {
            if (index > MaxPolygonCount) throw new Exception("Maximum vertex count reached for polygon");
            return _vertices[index];
        }
        set
        {
            if (index > MaxPolygonCount) throw new Exception("Maximum vertex count reached for polygon");
            _vertices[index] = value;
        }
    }

    public void Reverse()
    {
        var vertexCount = Count;
        switch (vertexCount)
        {
           case 3:
           {
               (this[0], this[1]) = (this[1], this[0]);
               break;
           }
           case 4:
           {
               (this[0], this[1]) = (this[1], this[0]);
               (this[2], this[3]) = (this[3], this[2]);
               break;
           }
           default:
           {
               int i;
               Span<short> storage = stackalloc short[MaxPolygonCount];
               for (i = 0; i < vertexCount; i++) storage[i] = this[i];
               for (i = 0; i < vertexCount; i++) this[i] = storage[i];
               break;
           }
        }
    }

    public IEnumerator<short> GetEnumerator() => ((IEnumerable<short>)_vertices).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}