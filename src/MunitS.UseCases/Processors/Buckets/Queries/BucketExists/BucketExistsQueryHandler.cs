using MediatR;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByNameRepository;
using MunitS.Protos;
namespace MunitS.UseCases.Processors.Buckets.Queries.BucketExists;

public class UploadFileCommandHandler(IBucketByNameRepository bucketByNameRepository) : IRequestHandler<BucketExistsQuery, BucketExistsResponse>
{
    public async Task<BucketExistsResponse> Handle(BucketExistsQuery query, CancellationToken cancellationToken)
    {
        var bucket = await bucketByNameRepository.Get(query.Request.BucketName);

        return new BucketExistsResponse
        {
            Exists = bucket != null
        };
    }
}
