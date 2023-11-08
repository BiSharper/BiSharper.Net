namespace BiSharper.Rv.VFS.Model;

public interface IRvEntry
{
    public string Name { get; }
    public RvFilesystem Filesystem { get; }
    public IRvEntryHolder ParentContext { get; }
    public IRvDirectory? ParentDirectory => ParentContext as IRvDirectory;
    
}