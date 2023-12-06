using BiSharper.Rv.Param.AST.Statement;

namespace BiSharper.Rv.Param.Writer;

public static partial class ParamWriters
{
    public enum BiSharperParamFormat
    {
        FlashpointText,
        FlashpointBinary,
        EliteText,
        EliteBinary,
        Json
    }

    public static void WriteParameter(ParamParameter parameter, Stream output, BiSharperParamFormat format)
    {
        switch (format)
        {
            case BiSharperParamFormat.FlashpointText: WriteProperty_FlashpointText(parameter, output); break;
            case BiSharperParamFormat.FlashpointBinary:
                break;
            case BiSharperParamFormat.EliteText:
                break;
            case BiSharperParamFormat.EliteBinary:
                break;
            case BiSharperParamFormat.Json:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(format), format, null);
        }
    }
}