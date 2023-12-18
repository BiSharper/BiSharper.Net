namespace BiSharper.Rv.Param.AST.Value.Enumerable;

public interface IParamArray<out T> : IParamValue, IEnumerable<T>
    where T : IParamValue;