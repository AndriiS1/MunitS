using Cassandra.Mapping;
namespace MunitS.Domain.Prefix.PrefixByParentPrefix;

public class FolderPrefixByParentPrefixMapping : Mappings
{
    private const string TableName = "folder_prefixes_by_parent_prefix";

    public FolderPrefixByParentPrefixMapping()
    {
        For<Prefix.PrefixByParentPrefix.FolderPrefixByParentPrefix>()
            .TableName(TableName)
            .PartitionKey(c => c.BucketId)
            .Column(c => c.BucketId, cm => cm.WithName("bucket_id"))
            .Column(c => c.ParentPrefix, cm => cm.WithName("parent_prefix"))
            .Column(c => c.Prefix, cm => cm.WithName("prefix"));
    }
}
