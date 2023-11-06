using System.Collections.Concurrent;
using BiSharper.Rv.Bank;
using BiSharper.Rv.VFS.Model;

namespace BiSharper.Rv.VFS;

public sealed class RvFilesystem : IRvEntryHolder
{
    private readonly ConcurrentDictionary<string, FileBank> _contexts = new();
    private readonly ConcurrentDictionary<string, IRvEntry> _entries = new();

    public T? GetEntry<T>(string name) where T : class, IRvEntry
    {
        if (_entries.TryGetValue(name, out var entry))
        {
            return entry as T;
        }

        return null;
    }


    public void LoadBank(FileBank bank)
    {
        if (_contexts.Contains<>(bank))
        {
            throw new Exception("Supplied bank already exists in the filesystem.");
        }
        
    }
}