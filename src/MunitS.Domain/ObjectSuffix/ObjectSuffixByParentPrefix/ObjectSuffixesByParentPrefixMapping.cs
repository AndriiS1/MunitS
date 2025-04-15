using Cassandra.Mapping;
namespace MunitS.Domain.ObjectSuffix.ObjectSuffixByParentPrefix;

public class ObjectSuffixesByParentPrefixMapping : Mappings
{
    private const string TableName = "object_suffixes_by_parent_prefix";

    public ObjectSuffixesByParentPrefixMapping()
    {
        For<ObjectSuffixByParentPrefix>()
            .TableName(TableName)
            .PartitionKey(c => c.BucketId)
            .Column(c => c.Id, cm => cm.WithName("id"))
            .Column(c => c.CreatedAt, cm => cm.WithName("created_at"))
            .Column(c => c.BucketId, cm => cm.WithName("bucket_id"))
            .Column(c => c.ParentPrefix, cm => cm.WithName("parent_prefix"))
            .Column(c => c.Type, cm => cm.WithName("type"))
            .Column(c => c.MimeType, cm => cm.WithName("mime_type"))
            .Column(c => c.Suffix, cm => cm.WithName("suffix"));
    }
}
