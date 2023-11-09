using System.Collections.Concurrent;

namespace BiSharper.Rv.VFS.Model;

public interface IRvEntryHolder
{
    public ConcurrentBag<IRvEntry> Entries { get; }
    

    public T? GetEntry<T>(string name) where T : class, IRvEntry => 
        Entries.OfType<T>().FirstOrDefault(s => s.Name == name);
}

public static class RvEntryHolderExtensions
{
    public static IRvDirectory? GetDirectory(this IRvEntryHolder holder, string name) => holder.GetEntry<IRvDirectory>(name);
    
    public static IRvEntryHolder CreateDirectory(this IRvEntryHolder holder, string name)
    {
        if (!name.Contains('\\'))
        {
            if (GetDirectory(holder, name) is { } dir) return dir;
            dir = new RvDirectory
            {
                Name = name,
                ParentContext = holder,
                Entries = new ConcurrentBag<IRvEntry>()
            };
            holder.Entries.Add(dir);
            return dir;
        }
        var sections = name.Split('\\');

        return sections.Aggregate(holder, (current, section) => current.CreateDirectory(section));
    }

}