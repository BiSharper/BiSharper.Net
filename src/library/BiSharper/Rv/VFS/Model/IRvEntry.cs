namespace BiSharper.Rv.VFS.Model;

public interface IRvEntry
{
    public string Name { get; protected init; }
    public IRvEntryHolder ParentContext { get; }
    public IRvDirectory? ParentDirectory => ParentContext as IRvDirectory;
    
}