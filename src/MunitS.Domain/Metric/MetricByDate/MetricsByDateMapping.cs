using Cassandra.Mapping;
namespace MunitS.Domain.Metric.MetricByDate;

public class MetricsByDateMapping : Mappings
{
    private const string TableName = "metrics_by_date";

    public MetricsByDateMapping()
    {
        For<MetricByDate>()
            .PartitionKey(c => c.BucketId, c => c.Date)
            .Column(c => c.BucketId, cm => cm.WithName("bucket_id"))
            .Column(c => c.Type, cm => cm.WithName("type"))
            .Column(c => c.Id, cm => cm.WithName("id"))
            .Column(c => c.Operation, cm => cm.WithName("operation"))
            .Column(c => c.Time, cm => cm.WithName("time"))
            .TableName(TableName);
    }
}
