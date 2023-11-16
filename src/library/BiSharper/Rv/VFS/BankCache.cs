using System.Diagnostics;
using BiSharper.Rv.Bank;
using BiSharper.Rv.Bank.Models;

namespace BiSharper.Rv.VFS;

public class BankCache 
{
    public readonly FileBank Bank;
    private Dictionary<BankEntry, byte[]> Cache { get; } = new ();

    public BankCache(FileBank bankFile) => Bank = bankFile;

    public byte[]? GetEntryData(BankEntry entry) => Cache.GetValueOrDefault(entry);
    public byte[] GetOrCreateEntryData(BankEntry entry) => GetEntryData(entry) ?? CreateEntryData(entry);
    public byte[] CreateEntryData(BankEntry entry)
    {
        Debug.Assert(entry.Owner == Bank);

        if (Cache.ContainsKey(entry))
        {
            throw new Exception("This entry already exists in the cache!");
        }


        return Cache[entry] = entry.Read() ?? throw new InvalidOperationException($"Failed to read data for entry \"{entry.Name}\".");
    }

}