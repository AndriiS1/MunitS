using Cassandra.Mapping;
namespace MunitS.Domain.Division.DivisionByBucketId;

public class DivisionsByBucketIdMapping : Mappings
{
    private const string TableName = "divisions_by_bucket_id";
    
    public DivisionsByBucketIdMapping()
    {
        For<DivisionByBucketId>()
            .PartitionKey(c => c.BucketId, c => c.Type)
            .Column(c => c.BucketId, cm => cm.WithName("bucket_id"))
            .Column(c => c.Type, cm => cm.WithName("type"))
            .Column(c => c.Name, cm => cm.WithName("name"))
            .Column(c => c.Id, cm => cm.WithName("id"))
            .Column(c => c.ObjectsCount, cm => cm.WithName("objects_count"))
            .Column(c => c.ObjectsLimit, cm => cm.WithName("objects_limit"))
            .Column(c => c.Path, cm => cm.WithName("path"))
            .TableName(TableName);
    }
}
