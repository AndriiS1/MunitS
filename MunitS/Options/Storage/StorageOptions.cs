namespace MunitS.Options.Storage;

public record StorageOptions
{
    public const string Section = "Storage";
    public required string RootDirectory { get; init; }
}
