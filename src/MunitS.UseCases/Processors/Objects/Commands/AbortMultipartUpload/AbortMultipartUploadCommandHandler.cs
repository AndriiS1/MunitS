using Grpc.Core;
using MediatR;
using MunitS.Domain.Rules;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByIdRepository;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByBucketIdRepository;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByFileKeyRepository;
using MunitS.Infrastructure.Data.Repositories.Part.PartByUploadId;
using MunitS.Protos;
using MunitS.UseCases.Processors.Objects.Services.MetadataBuilder;
using MunitS.UseCases.Processors.Objects.Services.ObjectBuilder;
using MunitS.UseCases.Processors.Service.PathRetriever;
using MunitS.UseCases.Processors.Service.PathRetriever.Dtos;
namespace MunitS.UseCases.Processors.Objects.Commands.AbortMultipartUpload;

public class AbortMultipartUploadCommandHandler(IObjectByBucketIdRepository objectByBucketIdRepository,
    IObjectByFileKeyRepository objectByFileKeyRepository,
    IBucketByIdRepository bucketByIdRepository,
    IObjectsBuilder objectsBuilder,
    IPathRetriever pathRetriever,
    IPartByUploadIdRepository partByUploadIdRepository,
    IMetadataBuilder metadataBuilder) : IRequestHandler<AbortMultipartUploadCommand, ObjectServiceStatusResponse>
{
    public async Task<ObjectServiceStatusResponse> Handle(AbortMultipartUploadCommand command, CancellationToken cancellationToken)
    {
        var bucket = await bucketByIdRepository.Get(Guid.Parse(command.Request.BucketId));

        if (bucket == null) throw new RpcException(new Status(StatusCode.NotFound, $"Bucket with name: {command.Request.BucketId} is not found."));

        var objectToAbort = await objectByBucketIdRepository.GetByUploadId(bucket.Id, Guid.Parse(command.Request.UploadId));

        if (objectToAbort is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "There is no instantiated object."));
        }

        var objects = (await objectByFileKeyRepository.GetAll(bucket.Id, objectToAbort.FileKey)).Where(o => o.UploadId != objectToAbort.UploadId);

        var objectDirectories = new ObjectDirectories(bucket.Name, objectToAbort);

        Directory.Delete(objects.Any() ?
            pathRetriever.GetAbsoluteDirectoryPath(objectDirectories.ObjectVersionDirectory) :
            pathRetriever.GetAbsoluteDirectoryPath(objectDirectories.ObjectDirectory), true);

        await Task.WhenAll(objectsBuilder
            .ToDelete(new ObjectsBuilder.DeleteObjectByBucketId(bucket.Id, objectToAbort.UploadId))
            .ToDelete(new ObjectsBuilder.DeleteObjectByParentPrefix(bucket.Id, FileKeyRule.GetFileName(objectToAbort.FileKey), FileKeyRule.GetParentPrefix(objectToAbort.FileKey)))
            .ToDelete(new ObjectsBuilder.DeleteObjectByFileKey(bucket.Id, objectToAbort.FileKey, objectToAbort.UploadId))
            .Build(), metadataBuilder
            .ToDelete(new MetadataBuilder.DeleteMetadataByObjectId(bucket.Id, objectToAbort.UploadId))
            .Build(), partByUploadIdRepository.Delete(bucket.Id, objectToAbort.UploadId));

        return new ObjectServiceStatusResponse
        {
            Status = "Success"
        };
    }
}
