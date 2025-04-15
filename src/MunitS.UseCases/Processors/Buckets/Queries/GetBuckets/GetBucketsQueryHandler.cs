using MediatR;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByIdRepository;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketCounter;
using MunitS.Protos;
using MunitS.UseCases.Processors.Buckets.Mappers;
namespace MunitS.UseCases.Processors.Buckets.Queries.GetBuckets;

public class UploadFileCommandHandler(IBucketByIdRepository bucketByIdRepository, IBucketCounterRepository bucketCounterRepository) : IRequestHandler<GetBucketsQuery, GetBucketsResponse>
{
    public async Task<GetBucketsResponse> Handle(GetBucketsQuery query, CancellationToken cancellationToken)
    {
        var buckets = await bucketByIdRepository.GetAll(query.Request.Ids.Select(c => new Guid(c)).ToArray());
        var bucketCounters = await bucketCounterRepository.GetAll(buckets.Select(b => b.Id));

        return ResponseMappers.FormatGetBucketsResponse(buckets.Select(b => (b, bucketCounters.FirstOrDefault(c => c.Id == b.Id))).ToList());
    }
}
