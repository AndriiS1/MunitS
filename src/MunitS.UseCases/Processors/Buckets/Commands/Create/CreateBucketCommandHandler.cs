using Grpc.Core;
using MediatR;
using MunitS.Domain.Bucket.BucketById;
using MunitS.Domain.Bucket.BucketByName;
using MunitS.Domain.Directory;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByIdRepository;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByNameRepository;
using MunitS.Protos;
using MunitS.UseCases.Processors.Service.PathRetriever;
using Directory = System.IO.Directory;
namespace MunitS.UseCases.Processors.Buckets.Commands.Create;

public class CreateBucketCommandHandler(IPathRetriever pathRetriever,
    IBucketByIdRepository bucketByIdRepository,
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

        var absoluteBucketDirectory = pathRetriever.GetAbsoluteBucketDirectory(new BucketDirectory(bucket.Name));

        if (!Directory.Exists(absoluteBucketDirectory))
        {
            Directory.CreateDirectory(absoluteBucketDirectory);
        }

        return new CreateBucketResponse
        {
            BucketId = bucket.Id.ToString()
        };
    }
}
