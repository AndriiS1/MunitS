using Cassandra.Mapping;
namespace MunitS.Domain.Division.DivisionCounter;

public class DivisionCountersMapping : Mappings
{
    public const string TableName = "division_counters";
    public const string ObjectsCountColumnName = "objects_count";

    public DivisionCountersMapping()
    {
        For<DivisionCounter>()
            .PartitionKey(c => c.BucketId, c => c.Type)
            .Column(c => c.BucketId, cm => cm.WithName("bucket_id"))
            .Column(c => c.Type, cm => cm.WithName("type"))
            .Column(c => c.Id, cm => cm.WithName("id"))
            .Column(c => c.ObjectsCount, cm => cm.WithName(ObjectsCountColumnName))
            .TableName(TableName);
    }
}
