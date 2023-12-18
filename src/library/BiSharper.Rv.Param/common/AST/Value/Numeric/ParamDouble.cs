namespace BiSharper.Rv.Param.AST.Value.Numeric;

public interface IParamDouble : IParamValue
{
    public double Value { get; }
}

public readonly struct ParamDouble : IParamDouble
{
    public double Value { get; }
    public ParamDouble(double value) => Value = value;
    public static explicit operator ParamDouble(double self) => new(self);
    public static implicit operator double(ParamDouble self) => self.Value;
}