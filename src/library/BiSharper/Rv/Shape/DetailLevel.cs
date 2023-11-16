using System.Numerics;
using System.Text;
using BiSharper.Rv.Shape.Flags;
using BiSharper.Rv.Shape.Types;
using BiSharper.Rv.Shape.Utils;

namespace BiSharper.Rv.Shape;

public class DetailLevel
{
    public RvShape Shape { get; private init; }
    public ShapeFace[] Faces { get; private init; }
    public Vector3[] Normals { get; private init; }
    public ShapePoint[] Points { get; private init; }
    public Vector2 InverseXY { get; private set; }
    public Vector2 MaximumXY { get; private set; }
    public Vector2 MinimumXY { get; private set; }
    public float Resolution { get; private init; }
    public Dictionary<string, string> Properties { get; private init; } = new Dictionary<string, string>();
    public int MinimumMaterial => Faces.Min(f => f.MinimumMaterial);
    public int FaceCount => Faces.Length;
    public int PointCount => Points.Length;

    public DetailLevel(
        BinaryReader reader,
        int shapeVersion,
        GeometryUsed geometryUsed,
        bool reversed,
        List<float> massArray, 
        RvShape parent)
    {
        Shape = parent;
        
        var signature = reader.ReadBytes(4);
        var headSize = reader.ReadInt32();
        var lodVersion = reader.ReadInt32();
        var pointCount = reader.ReadInt32();
        var normalCount = reader.ReadInt32();
        var faceCount = reader.ReadInt32();
        var flags = reader.ReadInt32();
        
        bool extended = false, material = false;
        if (signature == "P3DM"u8 || signature == "SP3X"u8)
        {
            extended = true;
            material = true;

            if (lodVersion != 0x100 && signature[0] == 'P')
            {
                throw new NotSupportedException($"Unsupported P3DM version {lodVersion}.");
            }

            reader.BaseStream.Seek(headSize - 28, SeekOrigin.Current);
        } else if (signature == "SP3D"u8)
        {
            //Our variables are all out of wack in accordance to this format - lets fix that.
            pointCount = headSize;
            headSize = 16;
            normalCount = lodVersion;
            lodVersion = 0;
            faceCount = pointCount;
            flags = 0;
        }

        Points = ShapePoint.ReadMulti(reader, extended, this, pointCount);
        Normals = reader.ReadMultipleVector3(normalCount);
        Faces = ShapeFace.ReadMulti(reader, geometryUsed, extended, material, this, faceCount);
        CalculateMinMaxes();
        
        if (reader.ReadBytes(4) != "TAGG"u8)
        {
            throw new Exception("Error encountered while finishing up LOD.");
        }

        TaggType tagg;
        while ((tagg = ReadingUtilities.ReadTag(reader, out var taggText, out var tagLength)) != TaggType.EndOfFile)
        {
            if(tagg == TaggType.EndOfFile) break;
            switch (tagg)
            {
                case TaggType.Mass:
                {
                    if (shapeVersion == 0)
                    {
                        //Warn:: "%s: Old mass no longer supported"
                        goto default;
                    }
                    
                    //TODO Mass calculations
                    reader.BaseStream.Seek(tagLength, SeekOrigin.Current);

                    break;
                }
                case TaggType.Animation:
                    //TODO Animation phases
                    reader.BaseStream.Seek(tagLength, SeekOrigin.Current);
                    break;
                case TaggType.UVSet:
                {
                    var stageId = reader.ReadInt32();
                    var bytesRead = 4;
                    if (stageId > 1)
                    {
                        //Warn:: "Warning: Unsupported UVSet {stageId}"
                    }

                    if (stageId != 1)
                    {
                        goto default;
                    }
                    //TODO UVSet Stage 1
                    break;
                }
                case TaggType.Property:
                {
                    var name = Encoding.ASCII.GetString(reader.ReadBytes(64)).TrimEnd(char.MinValue);
                    var value = Encoding.ASCII.GetString(reader.ReadBytes(64)).TrimEnd(char.MinValue);
                    Properties[name] = value;
                    break;
                }
                case TaggType.MaterialIndex:
                    Properties["__ambient"] = reader.ReadInt32().ToString("x8");
                    Properties["__diffuse"] = reader.ReadInt32().ToString("x8");
                    Properties["__specular"] = reader.ReadInt32().ToString("x8");
                    Properties["__emissive"] = reader.ReadInt32().ToString("x8");
                    break;
                case TaggType.NamedSelection:
                    break;
                case TaggType.Unknown:
                default:
                {
                    reader.BaseStream.Seek(tagLength, SeekOrigin.Current);
                    break;
                }
            }
        }

        Resolution = reader.ReadSingle();
    }

    private void CalculateMinMaxes()
    {
        const double minIntervalSize = 1e-6;
        Vector2 minXY = new(float.MaxValue), maxXY = new(float.MinValue);
        
        foreach (var face in Faces)
        {
            foreach (var vertex in face.Vertices)
            {
                if (vertex.X > maxXY.X) maxXY.X = vertex.X;
                if (vertex.Y > maxXY.Y) maxXY.Y = vertex.Y;
                if (vertex.X > minXY.X) minXY.X = vertex.X;
                if (vertex.Y > minXY.Y) minXY.Y = vertex.Y;
            }
        }
        

        if (MinimumMaterial != 0)
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

            if (maxXY[i] - minXY[i] < minIntervalSize)
            {
                var input = (float)(minXY[i] + minIntervalSize);
                {
                    var bitsRepresentation = BitConverter.ToUInt32(BitConverter.GetBytes(input), 0);

                    if ((bitsRepresentation & 0x7F800000) == 0x7F800000)
                    {
                        if (bitsRepresentation != 0xFF800000) input += input;
                    }
                    else if (bitsRepresentation == 0x80000000)
                    {
                        input = 1.401298464e-45f;
                    }
                    else
                    {
                        if ((bitsRepresentation & 0x80000000) == 0)
                            bitsRepresentation++;
                        else bitsRepresentation--;
                        input = BitConverter.ToSingle(BitConverter.GetBytes(bitsRepresentation), 0);
                    }
                }


                maxXY[i] = input;
            }
        }

        MinimumXY = minXY;
        MaximumXY = maxXY;
        InverseXY = new Vector2(1 / MaximumXY[0] - MinimumXY[0], 1 / MaximumXY[1] - MinimumXY[1]);
    }

    private static Vector3[] ReadNormals(BinaryReader reader, int count, bool reversed, GeometryUsed geometryUsed)
    {
        var normals = reader.ReadMultipleVector3(count);
        AlignGeometryUsedWithNormals(geometryUsed, normals);
        return normals;
    }

    private static void AlignGeometryUsedWithNormals(GeometryUsed geometryUsed, Vector3[] normals)
    {
        if ((geometryUsed & GeometryUsed.NoNormals) != GeometryUsed.Default)
        {
            Array.Fill(normals, Vector3.UnitY);
        }
    }
}