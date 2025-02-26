using MediatR;
using MunitS.Infrastructure.Data.Repositories.Bucket;
using MunitS.Protos;
using MunitS.UseCases.Services.Buckets.Mappers;
namespace MunitS.UseCases.Services.Buckets.Queries.GetBuckets;

public class UploadFileCommandHandler(IBucketRepository bucketRepository): IRequestHandler<GetBucketsQuery, GetBucketsResponse>
{
    public async Task<GetBucketsResponse> Handle(GetBucketsQuery query, CancellationToken cancellationToken)
    {
        var buckets = await bucketRepository.GetAll(query.Request.BucketNames.ToArray());
        
        return ResponseMappers.FormatGetBucketsResponse(buckets);
    }
}
