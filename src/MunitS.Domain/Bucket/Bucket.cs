namespace MunitS.Domain.Bucket;

public class Bucket
{
    public const string TableName = "buckets";
    public const int MaxVersions = 10;
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required bool VersioningEnabled { get; init; }
    public required int VersionsLimit { get; init; }
    public required long ObjectsCount { get; init; }
    public required long SizeInBytes { get; init; }
    
    public static Bucket Create(string name, bool versioningEnabled, int versionsLimit)
    {
        return new Bucket
        {
            Id = Guid.NewGuid(),
            Name = name,
            VersioningEnabled = versioningEnabled,
            VersionsLimit = versionsLimit,
            ObjectsCount = 0,
            SizeInBytes = 0
        };
    }
}