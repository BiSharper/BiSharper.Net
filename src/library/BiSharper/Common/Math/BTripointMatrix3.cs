using System.Numerics;

namespace BiSharper.Common.Math;

public struct BTripointMatrix3
{
    public Vector3 X;
    public Vector3 Y;
    public Vector3 Z;

    public BTripointMatrix3(Vector3 x = default, Vector3 y = default, Vector3 z = default)
    {
        X = x;
        Y = y;
        Z = z;
    }

}