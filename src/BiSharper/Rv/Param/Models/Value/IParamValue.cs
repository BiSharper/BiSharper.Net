using BiSharper.Rv.Param.Models.Statement;

namespace BiSharper.Rv.Param.Models.Value;

public interface IParamValue
{
    public string ToText();
}


public interface IParamArray : IParamValue, IEnumerable<IParamValue>
{
    string IParamValue.ToText() =>
        '{' + string.Join(", ", this.Select(s => s.ToText())) + '}';
}