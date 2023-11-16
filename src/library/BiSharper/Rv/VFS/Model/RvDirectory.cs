using System.Collections.Concurrent;

namespace BiSharper.Rv.VFS.Model;

public interface IRvDirectory : IRvEntry, IRvEntryHolder
{
}

public sealed class RvDirectory : IRvDirectory
{
    public string Name { get; init; }
    public IRvEntryHolder ParentContext { get; }
    public ConcurrentBag<IRvEntry> Entries { get; } = new();

    public RvDirectory(string name, IRvEntryHolder parent)
    {
        Name = name;
        ParentContext = parent;
    }


}