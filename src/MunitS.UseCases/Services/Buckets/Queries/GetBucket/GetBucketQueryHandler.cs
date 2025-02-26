using Grpc.Core;
using MediatR;
using MunitS.Domain.Bucket;
using MunitS.Infrastructure.Data.Repositories.Bucket;
using MunitS.Protos;
using MunitS.UseCases.Services.Buckets.Mappers;
namespace MunitS.UseCases.Services.Buckets.Queries.GetBucket;

public class UploadFileCommandHandler(IBucketRepository bucketRepository): IRequestHandler<GetBucketQuery, GetBucketResponse>
{
    public async Task<GetBucketResponse> Handle(GetBucketQuery query, CancellationToken cancellationToken)
    {
        var bucket = await bucketRepository.Get(query.Request.BucketName);

        if (bucket == null)
        {
            throw new RpcException(
                new Status(StatusCode.NotFound, $"Bucket {query.Request.BucketName} does not exists.")
            );
        }
        
        await bucketRepository.Delete(query.Request.BucketName);
        
        return ResponseMappers.FormatGetBucketResponse(bucket);
    }
}
