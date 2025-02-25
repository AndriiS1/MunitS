namespace MunitS.Domain.Metadata;

public class Metadata
{
    public const string TableName = "metadata";
    public required Guid VersionId { get; init; }
    public required Guid ObjectId { get; init; }
    public required string ContentType { get; init; }
    public required long SizeInBytes { get; init; }
    public required bool IsDeleted { get; init; } = false;
    public required Dictionary<string, string> CustomMetadata { get; init; }
    public required Dictionary<string, string> Tags { get; init; }
    public required Dictionary<string, string> SearchableKeywords { get; init; }
    
    public static Metadata Create(Guid versionId, Guid objectId, string contentType, long sizeInBytes)
    {
        return new Metadata
        {
            VersionId = versionId,
            ObjectId = objectId,
            ContentType = contentType,
            SizeInBytes = sizeInBytes,
            IsDeleted = false,
            CustomMetadata = new Dictionary<string, string>(),
            Tags = new Dictionary<string, string>(),
            SearchableKeywords = new Dictionary<string, string>()
        };
    }
}
