using Grpc.Core;
using MediatR;
using MunitS.Domain.Bucket.BucketCounter;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByIdRepository;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketCounter;
using MunitS.Protos;
using MunitS.UseCases.Processors.Buckets.Mappers;
namespace MunitS.UseCases.Processors.Buckets.Queries.GetBucket;

public class UploadFileCommandHandler(IBucketByIdRepository bucketByIdRepository,
    IBucketCounterRepository bucketCounterRepository) : IRequestHandler<GetBucketQuery, GetBucketResponse>
{
    public async Task<GetBucketResponse> Handle(GetBucketQuery query, CancellationToken cancellationToken)
    {
        var bucket = await bucketByIdRepository.Get(new Guid(query.Request.Id));

        if (bucket == null)
        {
            throw new RpcException(
                new Status(StatusCode.NotFound, $"Bucket {query.Request.Id} does not exists.")
            );
        }

        var bucketCounter = await bucketCounterRepository.Get(bucket.Id);

        return ResponseMappers.FormatGetBucketResponse(bucket, bucketCounter ?? BucketCounter.Empty(bucket.Id));
    }
}
