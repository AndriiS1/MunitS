namespace MunitS.Domain.FolderPrefixes.FolderPrefixByParentPrefix;

public class FolderPrefixByParentPrefix
{
    public required Guid Id { get; init; }
    public required Guid BucketId { get; init; }
    public required string ParentPrefix { get; init; }
    public required string Prefix { get; init; }

    public static FolderPrefixByParentPrefix Create(Guid id, Guid bucketId, string parentPrefix, string prefix)
    {
        return new FolderPrefixByParentPrefix
        {
            Id = id,
            BucketId = bucketId,
            ParentPrefix = parentPrefix,
            Prefix = prefix
        };
    }
}
