namespace MunitS.Infrastructure.Data.Repositories.Metric.MetricByDate;

public interface IMetricByDateRepository
{
    Task<List<Domain.Metric.MetricByDate.MetricByDate>> GetAll(Guid bucketId);
    Task Create(Domain.Metric.MetricByDate.MetricByDate metric);
    Task Delete(Guid bucketId);
}
