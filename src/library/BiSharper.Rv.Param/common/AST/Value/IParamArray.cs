
using BiSharper.Rv.Param.AST.Statement;

namespace BiSharper.Rv.Param.AST.Value;

public interface IParamArray<T> : IEnumerable<T>, IParamValue
    where T : IParamValue
{
    public List<T> Values { get; }


}