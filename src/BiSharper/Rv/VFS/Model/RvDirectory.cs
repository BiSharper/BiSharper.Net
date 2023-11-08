using System.Collections.Concurrent;

namespace BiSharper.Rv.VFS.Model;

public interface IRvDirectory : IRvEntry, IRvEntryHolder
{
}

public sealed class RvDirectory : IRvDirectory
{
    public string Name { get; }
    public required RvFilesystem Filesystem { get; init; }
    public required IRvEntryHolder ParentContext { get; init;  }
    
    public ConcurrentBag<IRvEntry> Entries { get; }

}