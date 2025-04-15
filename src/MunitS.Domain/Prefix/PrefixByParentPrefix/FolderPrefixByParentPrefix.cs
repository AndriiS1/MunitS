namespace MunitS.Domain.Prefix.PrefixByParentPrefix;

public class FolderPrefixByParentPrefix
{
    public required Guid BucketId { get; init; }
    public required string ParentPrefix { get; init; }
    public required string Prefix { get; init; }

    public static FolderPrefixByParentPrefix Create(Guid bucketId, string parentPrefix, string prefix)
    {
        return new FolderPrefixByParentPrefix
        {
            BucketId = bucketId,
            ParentPrefix = parentPrefix,
            Prefix = prefix
        };
    }
}
