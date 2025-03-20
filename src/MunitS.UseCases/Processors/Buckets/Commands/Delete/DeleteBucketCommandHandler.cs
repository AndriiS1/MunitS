using Grpc.Core;
using MediatR;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByIdRepository;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByNameRepository;
using MunitS.Protos;
namespace MunitS.UseCases.Processors.Buckets.Commands.Delete;

public class UploadFileCommandHandler(IBucketByIdRepository bucketByIdRepository, IBucketByNameRepository bucketByNameRepository): IRequestHandler<DeleteBucketCommand, BucketServiceStatusResponse>
{
    public async Task<BucketServiceStatusResponse> Handle(DeleteBucketCommand command, CancellationToken cancellationToken)
    {
        var bucket = await bucketByIdRepository.Get(new Guid(command.Request.Id));

        if (bucket == null)
        {
            throw new RpcException(
                new Status(StatusCode.NotFound, $"Bucket {command.Request.Id} does not exists.")
            );
        }
        
        await bucketByIdRepository.Delete(bucket.Id);
        await bucketByNameRepository.Delete(bucket.Name);
        
        return new BucketServiceStatusResponse { Status = "Success" };
    }
}
