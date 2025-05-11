using MediatR;
using MunitS.Domain.Metric.MetricByDate;
using MunitS.Domain.ObjectSuffix.ObjectSuffixByParentPrefix;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketCounter;
using MunitS.Infrastructure.Data.Repositories.Metric.MetricByDate;
using MunitS.Infrastructure.Data.Repositories.ObjectSuffix.ObjectSuffixByParentPrefixRepository;
using MunitS.Infrastructure.Data.Repositories.ObjectSuffix.ObjectSuffixByParentPrefixRepository.Dtos;
using MunitS.Protos;
using MunitS.UseCases.Processors.Objects.Mappers;
namespace MunitS.UseCases.Processors.Objects.Queries.GetObjects;

public class UploadFileCommandHandler(IObjectSuffixByParentPrefixRepository objectSuffixByParentPrefixRepository,
    IMetricByDateRepository metricByDateRepository,
    IBucketCounterRepository bucketCounterRepository)
    : IRequestHandler<GetObjectsQuery, GetObjectsSuffixesResponse>
{
    public async Task<GetObjectsSuffixesResponse> Handle(GetObjectsQuery query, CancellationToken cancellationToken)
    {
        var bucketId = Guid.Parse(query.Request.BucketId);

        var cursor = query.Request.Cursor is null ? new ObjectSuffixesPage.ObjectSuffixesPageCursor(PrefixType.Directory, string.Empty) :
            new ObjectSuffixesPage.ObjectSuffixesPageCursor(Enum.Parse<PrefixType>(query.Request.Cursor.Type), query.Request.Cursor.Suffix);

        var page = await objectSuffixByParentPrefixRepository
            .GetPage(bucketId, query.Request.Prefix, query.Request.PageSize, cursor);

        await Task.WhenAll(metricByDateRepository.Create(MetricByDate.Create(bucketId, Operation.ListObjects)),
            bucketCounterRepository.IncrementTypeBOperationsCount(bucketId));

        return ObjectResponseMappers.FormatObjectSuffixes(page);
    }
}
