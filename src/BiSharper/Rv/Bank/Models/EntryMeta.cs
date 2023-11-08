namespace BiSharper.Rv.Bank.Models;

public sealed record EntryMeta
{
    public required EntryMime Mime { get; init; } = EntryMime.Decompressed;
    public required uint Length { get; init; }
    public required long Offset { get; set; }
    public required uint Timestamp { get; init; }
    public required uint BufferLength { get; init; }

    internal EntryMeta()
    {
            
    }
}