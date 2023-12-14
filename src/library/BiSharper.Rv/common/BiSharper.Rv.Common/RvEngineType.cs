namespace BiSharper.Rv.Common;

[Flags]
public enum RvEngineType
{
    RealVirtuality3 = 2,
    RealVirtuality4 = 4,
    EarlyEnfusion = 8,


    RealVirtuality = RealVirtuality3 | RealVirtuality4,
}