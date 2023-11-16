namespace BiSharper.Rv.Param.Models;

public enum ParamAccessMode
{
    ReadWrite, //Default
    ReadCreate, //Only allow adding new class members
    ReadOnly,
    ReadOnlyVerified //Apply CRC Test
}