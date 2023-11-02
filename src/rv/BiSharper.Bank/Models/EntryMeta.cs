namespace BiSharper.FileBank.Models;

public struct EntryMeta
{
    public  required EntryMime Mime { get; init; }
    public required uint Length { get; init; }
    public required ulong Offset { get; set; }
    public required uint Timestamp { get; init; }
    public required uint BufferLength { get; init; }
}