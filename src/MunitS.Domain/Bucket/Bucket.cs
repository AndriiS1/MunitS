namespace MunitS.Domain.Bucket;

public class Bucket
{
    public const string TableName = "buckets";
    private const int MaxVersions = 10;
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required bool VersioningEnabled { get; init; }
    public required int VersionsLimit { get; init; }
    public required long ObjectsCount { get; init; }
    
    public static Bucket Create(string name)
    {
        return new Bucket
        {
            Id = Guid.NewGuid(),
            Name = name,
            VersioningEnabled = true,
            VersionsLimit = MaxVersions,
            ObjectsCount = 0
        };
    }
}