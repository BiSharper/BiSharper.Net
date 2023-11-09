using System.Collections.Concurrent;

namespace BiSharper.Rv.VFS.Model;

public interface IRvDirectory : IRvEntry, IRvEntryHolder
{
}

public sealed class RvDirectory : IRvDirectory
{
    public required string Name { get; init; }
    public required IRvEntryHolder ParentContext { get; init;  }
    public required ConcurrentBag<IRvEntry> Entries { get; init; } = new();

}