using Cassandra.Mapping;
namespace MunitS.Domain.Object.ObjectByFileKey;

public class ObjectsByFileKeyMapping : Mappings
{
    private const string TableName = "objects_by_file_key";

    public ObjectsByFileKeyMapping()
    {
        For<ObjectByFileKey>()
            .TableName(TableName)
            .PartitionKey(c => c.BucketId)
            .Column(c => c.Id, cm => cm.WithName("id"))
            .Column(c => c.BucketId, cm => cm.WithName("bucket_id"))
            .Column(c => c.CreatedAt, cm => cm.WithName("created_at"))
            .Column(c => c.FileKey, cm => cm.WithName("file_key"));
    }
}
