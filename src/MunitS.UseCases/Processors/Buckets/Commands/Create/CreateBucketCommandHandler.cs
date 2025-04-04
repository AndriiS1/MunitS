using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Options;
using MunitS.Domain.Bucket.BucketById;
using MunitS.Domain.Bucket.BucketByName;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByIdRepository;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByNameRepository;
using MunitS.Infrastructure.Options.Storage;
using MunitS.Protos;
namespace MunitS.UseCases.Processors.Buckets.Commands.Create;

public class UploadFileCommandHandler(IOptions<StorageOptions> storageOptions, IBucketByIdRepository bucketByIdRepository,
    IBucketByNameRepository bucketByNameRepository) 
    : IRequestHandler<CreateBucketCommand, CreateBucketResponse>
{
    public async Task<CreateBucketResponse> Handle(CreateBucketCommand command, CancellationToken cancellationToken)
    {
        var existingBucket = await bucketByNameRepository.Get(command.Request.BucketName);

        if (existingBucket != null)
        {
            throw new RpcException(
                new Status(StatusCode.AlreadyExists, $"Bucket {command.Request.BucketName} is already exists.")
            );
        }
        
        if (command.Request.VersionsLimit > BucketById.MaxVersions)
        {
            throw new RpcException(
                new Status(StatusCode.InvalidArgument, $"Max versions limit is {BucketById.MaxVersions} instances.")
            );
        }
        
        var bucket = BucketById.Create(command.Request.BucketName, command.Request.VersioningEnabled, command.Request.VersionsLimit);
        
        await bucketByIdRepository.Create(bucket);
        await bucketByNameRepository.Create(BucketByName.Create(bucket.Id, command.Request.BucketName));
        
        var bucketDirectory = new BucketDirectory(storageOptions.Value.RootDirectory, bucket);
        
        if (!Directory.Exists(bucketDirectory.Value))
        {
            Directory.CreateDirectory(bucketDirectory.Value);  
        }
        
        return new CreateBucketResponse
        {
            BucketId = bucket.Id.ToString()
        };
    }
}
