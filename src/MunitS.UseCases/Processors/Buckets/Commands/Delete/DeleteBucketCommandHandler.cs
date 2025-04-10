using Grpc.Core;
using MediatR;
using MunitS.Domain.Directory;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByIdRepository;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByNameRepository;
using MunitS.Protos;
using MunitS.UseCases.Processors.Service.PathRetriever;
namespace MunitS.UseCases.Processors.Buckets.Commands.Delete;

public class UploadFileCommandHandler(IBucketByIdRepository bucketByIdRepository, 
    IBucketByNameRepository bucketByNameRepository, IPathRetriever pathRetriever): IRequestHandler<DeleteBucketCommand, BucketServiceStatusResponse>
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
        
        var bucketDirectory = new BucketDirectory(bucket.Name);
        var absoluteBucketDirectory = pathRetriever.GetAbsoluteDirectoryPath(bucketDirectory);
        
        await bucketByIdRepository.Delete(bucket.Id);
        await bucketByNameRepository.Delete(bucket.Name);
        
        Directory.Delete(absoluteBucketDirectory, recursive: true);
        
        return new BucketServiceStatusResponse { Status = "Success" };
    }
}
