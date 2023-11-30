using System.Numerics;
using BiSharper.Rv.Shape.Flags;

namespace BiSharper.Rv.Render.Shape.Types;

public struct ShapeVertex
{
    public ShapeFace Face { get; private init; }
    public int PointIndex { get; private init; }
    public int NormalIndex { get; private init; }
    public float X { get; private set; }
    public float Y { get; private set; }
    public const float DefaultBounds = 100.0f;
    
    public ShapeVertex(BinaryReader reader, GeometryUsed geometryUsed, ShapeFace face)
    {
        Face = face;
        PointIndex = reader.ReadInt32();
        NormalIndex = reader.ReadInt32();
        X = reader.ReadSingle(); 
        Y = reader.ReadSingle();
        FixAndAlignXY(geometryUsed);
    }

    public void FixAndAlignXY(GeometryUsed geometryUsed, float xyLimit = DefaultBounds)
    {
        if ((geometryUsed & GeometryUsed.NoXY) != GeometryUsed.Default) X = Y = 0;
        float x = X, y = Y, absX = Math.Abs(x), absY = Math.Abs(y);
        if (absX >= 1e5 || absY >= 1e5 || !float.IsFinite(absX) || !float.IsFinite(absY))
        {
            //Warn:: "Face %d, point %d, face points %d,%d,%d - invalid uv %g,%g"
            x = y = 0;
        }

            
        if (absX > xyLimit || absY > xyLimit)
        {
            //Warn::  "UV coordinate on point %d is too big UV(%f, %f) - the UV compression may produce inaccurate results"
        }

        X = x;
        Y = y;
    }
    
    public static implicit operator Vector2(ShapeVertex vertex) => new(vertex.X, vertex.Y);


    public static ShapeVertex[] ReadMulti(BinaryReader reader, GeometryUsed geometryUsed, ShapeFace face, int count)
    {
        var vertices = new ShapeVertex[count];
        for (var i = 0; i < count; i++) vertices[i] = new ShapeVertex(reader, geometryUsed, face);
        return vertices;
    }
}