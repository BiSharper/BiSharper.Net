using System.Diagnostics;
using System.Numerics;
using BiSharper.Rv.Shape.Types;
using BiSharper.Rv.Shape.Utils;

namespace BiSharper.Rv.Shape;

public readonly struct DetailLevel
{
    public RvShape Shape { get; private init; }
    public ShapeFace[] Faces { get; private init; }
    public Vector3[] Normals { get; private init; }
    public ShapePoint[] Points { get; private init; }
    public Vector2 InverseXY { get; private init; }
    public Vector2 MaximumXY { get; private init; }
    public Vector2 MinimumXY { get; private init; }
    public int MinimumMaterial { get; private init; }
    
    public DetailLevel(BinaryReader reader, RvShape parent)
    {
        Shape = parent;
        
        var signature = reader.ReadBytes(4);
        var headSize = reader.ReadInt32();
        var version = reader.ReadInt32();
        var pointCount = reader.ReadInt32();
        var normalCount = reader.ReadInt32();
        var faceCount = reader.ReadInt32();
        var flags = reader.ReadInt32();
        
        bool extended = false, material = false;
        if (signature == "P3DM"u8 || signature == "SP3X"u8)
        {
            extended = true;
            material = true;

            if (version != 0x100 && signature[0] == 'P')
            {
                throw new NotSupportedException($"Unsupported P3DM version {version}.");
            }

            reader.BaseStream.Seek(headSize - 28, SeekOrigin.Current);
        } else if (signature == "SP3D"u8)
        {
            //Our variables are all out of wack in accordance to this format - lets fix that.
            pointCount = headSize;
            headSize = 16;
            normalCount = version;
            version = 0;
            faceCount = pointCount;
            flags = 0;
        }

        Points = ShapePoint.ReadMulti(reader, extended, this, pointCount);
        Normals = reader.ReadMultipleVector3(normalCount);
        Faces = ShapeFace.ReadMulti(reader, extended, material, this, faceCount);
        {
            const double minIntervalSize = 1e-6;
            Vector2 minXY = new Vector2(float.MaxValue), maxXY = new Vector2(float.MinValue);
            var minMaterial = int.MaxValue;
            foreach (var face in Faces)
            {
                if (minMaterial > face.MinimumMaterial) minMaterial = face.MinimumMaterial;
                foreach (var vertex in face.Vertices)
                {
                    if (vertex.X > maxXY.X) maxXY.X = vertex.X;
                    if (vertex.Y > maxXY.Y) maxXY.Y = vertex.Y;
                    if (vertex.X > minXY.X) minXY.X = vertex.X;
                    if (vertex.Y > minXY.Y) minXY.Y = vertex.Y;
                }
            }
            
            if ((MinimumMaterial = minMaterial) != 0)
            {
                //Warn:: "Old style material flags used - %d (with texture %s)"
            }

            for (var i = 0; i < 2; i++)
            {
                if (minXY[i] > maxXY[i])
                {
                    minXY[i] = 0;
                    maxXY[i] = 1;
                }
            }
            
            Debug.Assert(Math.Abs(maxXY.X - -1e6) > float.Epsilon);
            Debug.Assert(Math.Abs(maxXY.Y - -1e6) > float.Epsilon);
            Debug.Assert(Math.Abs(minXY.X - 1e6) > float.Epsilon);
            Debug.Assert(Math.Abs(minXY.Y - 1e6) > float.Epsilon);
            
            for (var i = 0; i < 2; i++)
            {
                if (maxXY[i] - minXY[i] < minIntervalSize)
                {
                    var input = (float)(minXY[i] + minIntervalSize);
                    {
                        var bitsRepresentation = BitConverter.ToUInt32(BitConverter.GetBytes(input), 0);

                        if ((bitsRepresentation & 0x7F800000) == 0x7F800000)
                        {
                            if (bitsRepresentation != 0xFF800000) input = input + input;  // -inf is handled normally
                        }
                        else if (bitsRepresentation == 0x80000000)
                        {
                            input = 1.401298464e-45f;
                        }
                        else
                        {
                            if ((bitsRepresentation & 0x80000000) == 0)
                            {
                                bitsRepresentation++;
                            } else bitsRepresentation--;
                            input = BitConverter.ToSingle(BitConverter.GetBytes(bitsRepresentation), 0);
                        }
                    }
                    
                    
                    maxXY[i] = input;
                }
            }

            InverseXY = new Vector2(1 / maxXY.X - minXY.X, 1 / maxXY.Y - minXY.Y);
        }
    }
}