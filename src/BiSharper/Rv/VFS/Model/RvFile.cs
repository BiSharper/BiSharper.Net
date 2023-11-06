namespace BiSharper.Rv.VFS.Model;

public class RvFile : IRvEntry
{
    public required RvFilesystem Filesystem { get; init;  }
    public required IRvEntryHolder ParentContext { get; init;  }

    public readonly MemoryStream FileData;

    public RvFile(MemoryStream fileData)
    {
        FileData = fileData;
    }
}