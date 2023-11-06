using System.Collections.Concurrent;

namespace BiSharper.Rv.VFS.Model;

public interface IRvDirectory : IRvEntry, IRvEntryHolder
{
}

public sealed class RvDirectory : IRvDirectory
{
    public required RvFilesystem Filesystem { get; init; }
    public required IRvEntryHolder ParentContext { get; init;  }

    private readonly ConcurrentDictionary<string, IRvEntry> _entries = new();

    public T? GetEntry<T>(string name) where T : class, IRvEntry
    {
        if (_entries.TryGetValue(name, out var entry))
        {
            return entry as T;
        }
        return null;
    }
}