using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Options;
using MunitS.Domain.Bucket;
using MunitS.Infrastructure.Data.Repositories.Bucket;
using MunitS.Infrastructure.Options.Storage;
using MunitS.Protos;
namespace MunitS.UseCases.Processors.Buckets.Commands.Create;

public class UploadFileCommandHandler(IOptions<StorageOptions> storageOptions, IBucketRepository bucketRepository) 
    : IRequestHandler<CreateBucketCommand, BucketServiceStatusResponse>
{
    public async Task<BucketServiceStatusResponse> Handle(CreateBucketCommand command, CancellationToken cancellationToken)
    {
        var existingBucket = await bucketRepository.Get(command.Request.BucketName);

        if (existingBucket != null)
        {
            throw new RpcException(
                new Status(StatusCode.AlreadyExists, $"Bucket {command.Request.BucketName} is already exists.")
            );
        }
        
        if (command.Request.VersionsLimit > Bucket.MaxVersions)
        {
            throw new RpcException(
                new Status(StatusCode.InvalidArgument, $"Max versions limit is {Bucket.MaxVersions} instances.")
            );
        }
        
        var bucket = Bucket.Create(command.Request.BucketName, command.Request.VersioningEnabled, command.Request.VersionsLimit);
        
        await bucketRepository.Create(bucket);
        
        var objectDirectory = new BucketDirectory(storageOptions.Value.RootDirectory, command.Request.BucketName);
        
        if (!Directory.Exists(objectDirectory.Value))
        {
            Directory.CreateDirectory(objectDirectory.Value);  
        }
        
        return new BucketServiceStatusResponse { Status = "Success" };
    }
}
