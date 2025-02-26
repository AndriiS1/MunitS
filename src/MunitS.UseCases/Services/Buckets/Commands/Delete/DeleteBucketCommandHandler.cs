using Grpc.Core;
using MediatR;
using MunitS.Infrastructure.Data.Repositories.Bucket;
using MunitS.Protos;
namespace MunitS.UseCases.Services.Buckets.Commands.Delete;

public class UploadFileCommandHandler(IBucketRepository bucketRepository): IRequestHandler<DeleteBucketCommand, BucketServiceStatusResponse>
{
    public async Task<BucketServiceStatusResponse> Handle(DeleteBucketCommand command, CancellationToken cancellationToken)
    {
        var bucket = await bucketRepository.Get(command.Request.BucketName);

        if (bucket != null)
        {
            throw new RpcException(
                new Status(StatusCode.NotFound, $"Bucket {command.Request.BucketName} does not exists.")
            );
        }
        
        await bucketRepository.Delete(command.Request.BucketName);
        
        return new BucketServiceStatusResponse { Status = "Success" };
    }
}
