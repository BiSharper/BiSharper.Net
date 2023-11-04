using System.Collections.Concurrent;
using BiSharper.Rv.VFS.Model;

namespace BiSharper.Rv.VFS;

public sealed class RvFilesystem : IRvDirectory
{
    public RvFilesystem Filesystem { get; }
    public IEnumerable<IEntry> Entries { get; }

    private readonly ConcurrentBag<IEntry> _entries = new();

}