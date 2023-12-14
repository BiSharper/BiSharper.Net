using System.Collections.Concurrent;
using BiSharper.Rv.Param.AST.Abstraction;
using BiSharper.Rv.Param.AST.Statement;
using BiSharper.Rv.Param.AST.Value;

namespace SerializationScratch;

public partial struct CfgPatch : IParamContext
{
    private readonly List<IParamComputableStatement> _computableStatements = [];
    private readonly ConcurrentDictionary<string, ParamParameter> _parameters = new();
    private readonly ConcurrentDictionary<string, IParamContext> _classes = new();

    private ParamArray<ParamString> _unitsValue = new ParamArray<ParamString>();
    private readonly ParamParameter _unitsParameter;

    private ParamArray<ParamString> _weaponsValue = new ParamArray<ParamString>();
    private readonly ParamParameter _weaponsParameter;

    //by ref for systypes that should be editable
    private ParamFloat _requiredVersionValue = new ParamFloat();
    private readonly ParamParameter _requiredVersionParameter;

    private ParamArray<ParamString> _requiredAddonsValue = new ParamArray<ParamString>();
    private readonly ParamParameter _requiredAddonsParameter;

    public IEnumerable<IParamStatement> Statements
    {
        get
        {
            yield return _unitsParameter;
            yield return _weaponsParameter;
            yield return _requiredVersionParameter;
            yield return _requiredAddonsParameter;
            foreach (var statement in _computableStatements) yield return statement;
            foreach (var (_, @class) in _classes) yield return @class;
            foreach (var (_, @param) in _classes) yield return @param;
        }
    }


    public partial IEnumerable<string> Units()
    {
        foreach (var unit in _unitsValue)
        {
            yield return unit;
        }
    }


    public partial IEnumerable<string> Weapons()
    {
        foreach (var weapon in _weaponsValue)
        {
            yield return weapon;
        }
    }

    public partial IEnumerable<string> RequiredAddons()
    {
        foreach (var requiredAddon in _requiredAddonsValue)
        {
            yield return requiredAddon;
        }
    }

    public partial float RequiredVersion() => _requiredVersionValue;


    public CfgPatch()
    {
        _unitsParameter = new ParamParameter("units", _unitsValue, this);
        _weaponsParameter = new ParamParameter("weapons", _weaponsValue, this);
        _requiredVersionParameter = new ParamParameter("requiredVersion", _requiredVersionValue, this);
        _requiredAddonsParameter = new ParamParameter("requiredAddons", _requiredAddonsValue, this);
    }

}