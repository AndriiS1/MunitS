using Cassandra.Data.Linq;
namespace MunitS.Infrastructure.Data.Repositories.Metric.MetricByDate;

public class MetricByDateRepository(CassandraConnector connector) : IMetricByDateRepository
{
    private readonly Table<Domain.Metric.MetricByDate.MetricByDate> _metrics = new(connector.GetSession());

    public async Task<List<Domain.Metric.MetricByDate.MetricByDate>> GetAll(Guid bucketId)
    {
        return (await _metrics.Where(b => b.BucketId == bucketId).ExecuteAsync()).ToList();
    }

    public async Task Create(Domain.Metric.MetricByDate.MetricByDate metric)
    {
        await _metrics.Insert(metric).ExecuteAsync();
    }

    public async Task Delete(Guid id)
    {
        await _metrics.Where(b => b.Id == id).Delete().ExecuteAsync();
    }
}
