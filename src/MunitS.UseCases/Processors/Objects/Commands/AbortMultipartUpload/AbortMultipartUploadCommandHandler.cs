using Grpc.Core;
using MediatR;
using MunitS.Domain.Directory.Dtos;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByIdRepository;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByFileKeyRepository;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByUploadIdRepository;
using MunitS.Infrastructure.Data.Repositories.Part.PartByUploadId;
using MunitS.Protos;
using MunitS.UseCases.Processors.Objects.Services.ObjectBuilder;
using MunitS.UseCases.Processors.Service.PathRetriever;
namespace MunitS.UseCases.Processors.Objects.Commands.AbortMultipartUpload;

public class AbortMultipartUploadCommandHandler(IObjectByUploadIdRepository objectByUploadIdRepository,
    IObjectByFileKeyRepository objectByFileKeyRepository,
    IBucketByIdRepository bucketByIdRepository,
    IObjectsBuilder objectsBuilder,
    IPathRetriever pathRetriever,
    IPartByUploadIdRepository partByUploadIdRepository)
    : IRequestHandler<AbortMultipartUploadCommand, ObjectServiceStatusResponse>
{
    public async Task<ObjectServiceStatusResponse> Handle(AbortMultipartUploadCommand command, CancellationToken cancellationToken)
    {
        var bucket = await bucketByIdRepository.Get(Guid.Parse(command.Request.BucketId));

        if (bucket == null) throw new RpcException(new Status(StatusCode.NotFound, $"Bucket with name: {command.Request.BucketId} is not found."));

        var objectToAbort = await objectByUploadIdRepository.GetByUploadId(bucket.Id, Guid.Parse(command.Request.ObjectId), Guid.Parse(command.Request.UploadId));

        if (objectToAbort is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "There is no instantiated object."));
        }

        var objects = (await objectByFileKeyRepository.GetAll(bucket.Id, objectToAbort.FileKey)).Where(o => o.UploadId != objectToAbort.UploadId);

        var objectDirectories = new ObjectVersionDirectories(bucket.Name, objectToAbort);

        Directory.Delete(objects.Any() ?
            pathRetriever.GetAbsoluteDirectoryPath(objectDirectories.ObjectVersionDirectory) :
            pathRetriever.GetAbsoluteDirectoryPath(objectDirectories.ObjectDirectory), true);

        await Task.WhenAll(objectsBuilder
            .ToDelete(new ObjectsBuilder.DeleteObjectByBucketId(bucket.Id, objectToAbort.UploadId))
            .ToDelete(new ObjectsBuilder.DeleteObjectByFileKey(bucket.Id, objectToAbort.FileKey, objectToAbort.UploadId))
            .Build(), partByUploadIdRepository.Delete(bucket.Id, objectToAbort.UploadId));

        return new ObjectServiceStatusResponse
        {
            Status = "Success"
        };
    }
}
