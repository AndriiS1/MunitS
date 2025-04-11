namespace MunitS.Domain.Bucket.BucketById;

public class BucketById
{
    public const int MaxVersions = 10;
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required bool VersioningEnabled { get; init; }
    public required int VersionsLimit { get; init; }

    public static BucketById Create(string name, bool versioningEnabled, int versionsLimit)
    {
        return new BucketById
        {
            Id = Guid.NewGuid(),
            Name = name,
            VersioningEnabled = versioningEnabled,
            VersionsLimit = versionsLimit
        };
    }
}
