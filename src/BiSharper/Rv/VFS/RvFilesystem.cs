using System.Collections.Concurrent;
using BiSharper.Rv.Bank;
using BiSharper.Rv.VFS.Model;

namespace BiSharper.Rv.VFS;

public sealed class RvFilesystem : IRvEntryHolder
{
    private readonly ConcurrentBag<FileBank> _contexts = new();
    public ConcurrentBag<IRvEntry> Entries { get; } = new();

    public void LoadBank(FileBank bank)
    {
        if (_contexts.Contains(bank))
        {
            throw new Exception("Supplied bank already exists in the filesystem.");
        }

        _contexts.Add(bank);
        this.CreateDirectory(bank.Prefix);
    }
    
    
    
}