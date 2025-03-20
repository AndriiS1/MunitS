using Cassandra.Mapping;
namespace MunitS.Domain.Bucket.BucketByName;

public class BucketsByNameMapping : Mappings
{
    private const string TableName = "buckets_by_name";
    
    public BucketsByNameMapping()
    {
        For<BucketByName>()
            .TableName(TableName)
            .PartitionKey(c => c.Name)
            .Column(c => c.Name, cm => cm.WithName("name"))
            .Column(c => c.Id, cm => cm.WithName("id"));
    }
}
