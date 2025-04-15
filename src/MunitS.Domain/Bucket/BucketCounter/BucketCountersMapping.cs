using Cassandra.Mapping;
namespace MunitS.Domain.Bucket.BucketCounter;

public class BucketCountersMapping : Mappings
{
    public const string TableName = "bucket_counters";
    public const string ObjectsCountColumnName = "objects_count";
    public const string SizeInBytesColumnName = "size_in_bytes";

    public BucketCountersMapping()
    {
        For<BucketCounter>()
            .TableName(TableName)
            .PartitionKey(c => c.Id)
            .Column(c => c.Id, cm => cm.WithName("id"))
            .Column(c => c.ObjectsCount, cm => cm.WithName(ObjectsCountColumnName))
            .Column(c => c.SizeInBytes, cm => cm.WithName(SizeInBytesColumnName));
    }
}
