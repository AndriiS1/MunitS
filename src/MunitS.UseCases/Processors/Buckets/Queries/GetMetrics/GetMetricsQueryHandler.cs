using Grpc.Core;
using MediatR;
using MunitS.Domain.Metric.MetricByDate;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByIdRepository;
using MunitS.Infrastructure.Data.Repositories.Metric.MetricByDate;
using MunitS.Protos;
namespace MunitS.UseCases.Processors.Buckets.Queries.GetMetrics;

public class UploadFileCommandHandler(IBucketByIdRepository bucketByIdRepository, IMetricByDateRepository metricRepository) : IRequestHandler<GetMetricsQuery, GetMetricsResponse>
{
    public async Task<GetMetricsResponse> Handle(GetMetricsQuery query, CancellationToken cancellationToken)
    {
        var bucketId = new Guid(query.Request.BucketId);

        var bucket = await bucketByIdRepository.Get(bucketId);

        if (bucket == null)
        {
            throw new RpcException(
                new Status(StatusCode.NotFound, $"Bucket {query.Request.BucketId} does not exists.")
            );
        }

        var metrics = await metricRepository.GetAll(bucketId);

        var groupedMetrics = metrics
            .GroupBy(m => m.Date)
            .ToDictionary(g => g.Key, g => g.ToList())
            .OrderBy(c => c.Key)
            .Select(g => new MetricResponse
            {
                Date = g.Key.ToString(),
                TypeAOperationsCount = g.Value.Count(x => x.Type == nameof(OperationType.A)),
                TypeBOperationsCount = g.Value.Count(x => x.Type == nameof(OperationType.B))
            });

        var response = new GetMetricsResponse();

        response.Metrics.AddRange(groupedMetrics);

        return response;
    }
}
