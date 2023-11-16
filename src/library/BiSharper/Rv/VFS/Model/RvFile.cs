namespace BiSharper.Rv.VFS.Model;


public class RvFile : IRvEntry
{
    public string Name { get; init; }
    public required IRvEntryHolder ParentContext { get; init;  }

    public readonly Stream FileData;

    public RvFile(string name, Stream fileData)
    {
        FileData = fileData;
        Name = name;
    }
}