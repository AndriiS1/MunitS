namespace MunitS.Infrastructure.Options.Versioning;

public class VersioningOptions
{
    public const string Section = "Versioning";
    public required bool Enabled { get; set; }
    public required uint VersionsLimit { get; set; }
}
