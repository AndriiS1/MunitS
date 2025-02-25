namespace MunitS.Domain.Versioning;

public class ObjectVersion
{
    public const string TableName = "object_versions";
    public required Guid Id { get; init; }
    public required Guid ObjectId { get; init; }
    public required DateTimeOffset UploadedAt { get; init; }

    public static ObjectVersion Create(Guid objectId, DateTimeOffset uploadedAt)
    {
        return new ObjectVersion
        {
            Id = Guid.NewGuid(),
            ObjectId = objectId,
            UploadedAt = uploadedAt
        };
    }
}
