using BiSharper.Rv.Bank.Models;

namespace BiSharper.Rv.VFS.Model;

public class RvBankFile : RvFile
{
    public readonly BankEntry BankEntry;
    
    public RvBankFile(string name, BankEntry entry) : base(name, new MemoryStream(entry.Read() ?? throw new InvalidOperationException("Failed to read entry.")))
    {
        BankEntry = entry;
    }
}