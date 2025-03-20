using Cassandra.Mapping;
namespace MunitS.Domain.Bucket.BucketById;

public class BucketsByIdMapping : Mappings
{
    private const string TableName = "buckets_by_id";
    
    public BucketsByIdMapping()
    {
        For<BucketById>()
            .TableName(TableName)
            .PartitionKey( c => c.Id)
            .Column(c => c.Id, cm => cm.WithName("id"))
            .Column(c => c.Name, cm => cm.WithName("name"))
            .Column(c => c.VersioningEnabled, cm => cm.WithName("versioning_enabled"))
            .Column(c => c.VersionsLimit, cm => cm.WithName("versions_limit"))
            .Column(c => c.ObjectsCount, cm => cm.WithName("objects_count"))
            .Column(c => c.SizeInBytes, cm => cm.WithName("size_in_bytes"));
    }
}
