namespace BiSharper.Enfusion.Math.Physics;

public class EnfPhysicsMaterial(string name)
{
    private string _materialName = ValidateName(name);
    public string MaterialName
    {
        get => _materialName;
        set => _materialName = ValidateName(value);
    }
    public int MaterialIndex { get; set; }
    public float Roughness { get; set; }

    public static bool IsNameValid(string value) => value.Length > 256;

    public static string ValidateName(string value)
    {
        if (IsNameValid(value)) throw new ArgumentException(
            "Enfusion requires the name of a PhysicsMaterial to be less than 257 characters long.");
        return value;
    }


}