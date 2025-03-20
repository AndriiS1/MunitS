using MediatR;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByIdRepository;
using MunitS.Protos;
using MunitS.UseCases.Processors.Buckets.Mappers;
namespace MunitS.UseCases.Processors.Buckets.Queries.GetBuckets;

public class UploadFileCommandHandler(IBucketByIdRepository bucketByIdRepository): IRequestHandler<GetBucketsQuery, GetBucketsResponse>
{
    public async Task<GetBucketsResponse> Handle(GetBucketsQuery query, CancellationToken cancellationToken)
    {
        var buckets = await bucketByIdRepository.GetAll(query.Request.Ids.Select(c => new Guid(c)).ToArray());
        
        return ResponseMappers.FormatGetBucketsResponse(buckets);
    }
}
