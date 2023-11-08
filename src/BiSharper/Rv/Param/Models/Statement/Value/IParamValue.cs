namespace BiSharper.Rv.Param.Models.Statement.Value;

public interface IParamValue : IParamStatement
{
    public string ToText();
}


public interface IParamArray : IParamValue, IEnumerable<IParamValue>
{
    string IParamValue.ToText() =>
        '{' + string.Join(", ", this.Select(s => s.ToText())) + '}';
}