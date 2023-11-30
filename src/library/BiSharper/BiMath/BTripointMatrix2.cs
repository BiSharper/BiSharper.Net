namespace BiSharper.BiMath;

public readonly struct BTripointMatrix2(Vector3 u, Vector3 v)
{
    public Vector3 U { get; } = u;
    public Vector3 V { get; } = v;
}