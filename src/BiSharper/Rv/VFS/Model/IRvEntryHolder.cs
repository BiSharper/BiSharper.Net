using System.Collections.Concurrent;

namespace BiSharper.Rv.VFS.Model;

public interface IRvEntryHolder
{
    public ConcurrentBag<IRvEntry> Entries { get; }
    

    public T? GetEntry<T>(string name) where T : class, IRvEntry => 
        Entries.OfType<T>().FirstOrDefault(s => s.Name == name);
    public IRvDirectory? GetDirectory(string name) => GetEntry<IRvDirectory>(name);
}