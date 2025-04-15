namespace MunitS.Domain.ObjectSuffix.ObjectSuffixByParentPrefix;

public class ObjectSuffixByParentPrefix
{
    public required Guid Id { get; init; }
    public required Guid BucketId { get; init; }
    public required string ParentPrefix { get; init; }
    public required string Suffix { get; init; }
    public required PrefixType Type { get; init; }
    public string? MimeType { get; private init; }
    public DateTimeOffset CreatedAt { get; private init; }

    public static ObjectSuffixByParentPrefix Create(Guid bucketId, Guid id, string parentPrefix,
        string prefix, PrefixType prefixType, string? mimeType = null)
    {
        return new ObjectSuffixByParentPrefix
        {
            Id = id,
            BucketId = bucketId,
            ParentPrefix = parentPrefix,
            Suffix = prefix,
            Type = prefixType,
            MimeType = mimeType,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }
}
