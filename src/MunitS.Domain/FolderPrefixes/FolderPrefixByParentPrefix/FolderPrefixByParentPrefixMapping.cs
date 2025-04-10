using Cassandra.Mapping;
namespace MunitS.Domain.FolderPrefixes.FolderPrefixByParentPrefix;

public class FolderPrefixByParentPrefixMapping : Mappings
{
    private const string TableName = "folder_prefixes_by_parent_prefix";

    public FolderPrefixByParentPrefixMapping()
    {
        For<FolderPrefixByParentPrefix>()
            .TableName(TableName)
            .PartitionKey(c => c.BucketId, c => c.ParentPrefix)
            .Column(c => c.BucketId, cm => cm.WithName("bucket_id"))
            .Column(c => c.ParentPrefix, cm => cm.WithName("parent_prefix"))
            .Column(c => c.Prefix, cm => cm.WithName("prefix"));
    }
}
