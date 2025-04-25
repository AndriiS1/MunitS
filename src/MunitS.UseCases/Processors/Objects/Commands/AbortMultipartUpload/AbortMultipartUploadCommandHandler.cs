using Grpc.Core;
using MediatR;
using MunitS.Domain.Directory.Dtos;
using MunitS.Domain.Object.ObjectByUploadId;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByIdRepository;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByFileKeyRepository;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByUploadIdRepository;
using MunitS.Infrastructure.Data.Repositories.Part.PartByUploadId;
using MunitS.Protos;
using MunitS.UseCases.Processors.Objects.Services.ObjectDeletionService;
using MunitS.UseCases.Processors.Service.PathRetriever;
namespace MunitS.UseCases.Processors.Objects.Commands.AbortMultipartUpload;

public class AbortMultipartUploadCommandHandler(IObjectByUploadIdRepository objectByUploadIdRepository,
    IObjectByFileKeyRepository objectByFileKeyRepository,
    IBucketByIdRepository bucketByIdRepository,
    IObjectDeletionService objectDeletionService,
    IPathRetriever pathRetriever,
    IPartByUploadIdRepository partByUploadIdRepository)
    : IRequestHandler<AbortMultipartUploadCommand, ObjectServiceStatusResponse>
{
    public async Task<ObjectServiceStatusResponse> Handle(AbortMultipartUploadCommand command, CancellationToken cancellationToken)
    {
        var objectId = Guid.Parse(command.Request.ObjectId);

        var bucket = await bucketByIdRepository.Get(Guid.Parse(command.Request.BucketId));

        if (bucket == null) throw new RpcException(new Status(StatusCode.NotFound, $"Bucket with name: {command.Request.BucketId} is not found."));

        var objectToAbort = await objectByUploadIdRepository.GetByUploadId(bucket.Id, objectId, Guid.Parse(command.Request.UploadId));

        if (objectToAbort is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "There is no instantiated object."));
        }

        if (objectToAbort.UploadStatus == nameof(UploadStatus.Completed))
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Cannot abort already uploaded object."));
        }

        var allVersions = await objectByUploadIdRepository.GetAll(bucket.Id, objectId);

        List<Task> tasks = [objectByUploadIdRepository.Delete(bucket.Id, objectId, objectToAbort.UploadId), partByUploadIdRepository.Delete(bucket.Id, objectToAbort.UploadId)];

        var otherVersions = allVersions.Where(v => v.UploadId != objectToAbort.UploadId).ToList();

        if (otherVersions.Count == 0)
        {
            tasks.Add(objectByFileKeyRepository.Delete(bucket.Id, objectToAbort.FileKey));
            tasks.Add(objectDeletionService.DeleteObjectPrefixesRelations(bucket.Id, objectToAbort.FileKey));
        }

        await Task.WhenAll(tasks);

        var objectDirectories = new ObjectVersionDirectories(bucket.Name, objectToAbort);

        Directory.Delete(otherVersions.Count == 0
            ? pathRetriever.GetAbsoluteDirectoryPath(objectDirectories.ObjectDirectory)
            : pathRetriever.GetAbsoluteDirectoryPath(objectDirectories.ObjectVersionDirectory), true);

        return new ObjectServiceStatusResponse
        {
            Status = "Success"
        };
    }
}
