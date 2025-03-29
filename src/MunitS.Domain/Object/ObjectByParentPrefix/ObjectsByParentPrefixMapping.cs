using Cassandra.Mapping;
namespace MunitS.Domain.Object.ObjectByParentPrefix;

public class ObjectsByParentPrefixMapping : Mappings
{
    private const string TableName = "objects_by_parent_prefix";

    public ObjectsByParentPrefixMapping()
    {
        For<ObjectByParentPrefix>()
            .TableName(TableName)
            .PartitionKey(c => c.BucketId, c => c.ParentPrefix)
            .Column(c => c.Id, cm => cm.WithName("id"))
            .Column(c => c.BucketId, cm => cm.WithName("bucket_id"))
            .Column(c => c.FileName, cm => cm.WithName("file_name"))
            .Column(c => c.ParentPrefix, cm => cm.WithName("parent_prefix"))
            .Column(c => c.UploadedAt, cm => cm.WithName("uploaded_at"));
    }
}
