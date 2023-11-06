namespace BiSharper.Rv.VFS.Model;

public interface IRvEntryHolder
{
    public T? GetEntry<T>(string name) where T: class, IRvEntry;
    public IRvDirectory? GetDirectory(string name) => GetEntry<IRvDirectory>(name);
}