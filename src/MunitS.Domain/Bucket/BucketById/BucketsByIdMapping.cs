using Cassandra.Mapping;
namespace MunitS.Domain.Bucket.BucketById;

public class BucketsByIdMapping : Mappings
{
    public const string TableName = "buckets_by_id";
    public const string ObjectsCount = "objects_count";
    public const string SizeInBytes = "size_in_bytes";

    public BucketsByIdMapping()
    {
        For<BucketById>()
            .TableName(TableName)
            .PartitionKey(c => c.Id)
            .Column(c => c.Id, cm => cm.WithName("id"))
            .Column(c => c.Name, cm => cm.WithName("name"))
            .Column(c => c.VersioningEnabled, cm => cm.WithName("versioning_enabled"))
            .Column(c => c.VersionsLimit, cm => cm.WithName("versions_limit"))
            .Column(c => c.ObjectsCount, cm => cm.WithName(ObjectsCount))
            .Column(c => c.SizeInBytes, cm => cm.WithName(SizeInBytes));
    }
}
