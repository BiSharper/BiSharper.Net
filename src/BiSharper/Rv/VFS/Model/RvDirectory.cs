using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace BiSharper.Rv.VFS.Model;

public interface IRvDirectory
{
    public IEnumerable<IEntry> Entries { get; }

    
}

public sealed class RvDirectory : IEntry, IRvDirectory
{
    public RvFilesystem Filesystem { get; }
    public IEnumerable<IEntry> Entries => _entries;
    
    private readonly ConcurrentBag<IEntry> _entries = new();

}