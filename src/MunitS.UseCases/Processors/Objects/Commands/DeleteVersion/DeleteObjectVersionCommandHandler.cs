using Grpc.Core;
using MediatR;
using MunitS.Domain.Directory.Dtos;
using MunitS.Domain.Division.DivisionByBucketId;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByIdRepository;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketCounter;
using MunitS.Infrastructure.Data.Repositories.Division.DivisionCounters;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByFileKeyRepository;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByUploadIdRepository;
using MunitS.Protos;
using MunitS.UseCases.Processors.Objects.Services.ObjectDeletionService;
using MunitS.UseCases.Processors.Service.PathRetriever;
namespace MunitS.UseCases.Processors.Objects.Commands.DeleteVersion;

public class DeleteObjectVersionCommandHandler(
    IObjectByFileKeyRepository objectByFileKeyRepository,
    IBucketCounterRepository bucketCounterRepository,
    IDivisionCounterRepository divisionCounterRepository,
    IBucketByIdRepository bucketByIdRepository,
    IObjectByUploadIdRepository objectByUploadIdRepository,
    IPathRetriever pathRetriever,
    IObjectDeletionService objectDeletionService) : IRequestHandler<DeleteObjectVersionCommand, ObjectServiceStatusResponse>
{
    public async Task<ObjectServiceStatusResponse> Handle(DeleteObjectVersionCommand command, CancellationToken cancellationToken)
    {
        var versionId = Guid.Parse(command.Request.UploadId);

        var bucket = await bucketByIdRepository.Get(Guid.Parse(command.Request.BucketId));

        if (bucket == null) throw new RpcException(new Status(StatusCode.NotFound, $"Bucket with name: {command.Request.BucketId} is not found."));

        var objectToDelete = await objectByFileKeyRepository.Get(bucket.Id, command.Request.FileKey);

        if (objectToDelete is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "No object by file key found."));
        }

        var versions = await objectByUploadIdRepository.GetAll(bucket.Id, objectToDelete.Id);
        var versionToDelete = versions.FirstOrDefault(v => v.UploadId == versionId);
        var otherVersions = versions.Where(v => v.UploadId != versionId).ToList();

        if (versionToDelete is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Object version is not found."));
        }

        List<Task> tasks =
        [
            objectByUploadIdRepository.Delete(bucket.Id, objectToDelete.Id, versionToDelete.Id),
            divisionCounterRepository.IncrementObjectsCount(bucket.Id, Enum.Parse<DivisionType.SizeType>(versionToDelete.DivisionSizeType), versionToDelete.DivisionId, -1),
            bucketCounterRepository.IncrementObjectsCount(bucket.Id, -1),
            bucketCounterRepository.IncrementSizeInBytesCount(bucket.Id, -versionToDelete.SizeInBytes)
        ];

        if (otherVersions.Count == 0)
        {
            tasks.Add(objectByFileKeyRepository.Delete(bucket.Id, versionToDelete.FileKey));
            tasks.Add(objectDeletionService.DeleteObjectPrefixesRelations(bucket.Id, objectToDelete.FileKey));
        }

        await Task.WhenAll(tasks);

        var objectDirectories = new ObjectVersionDirectories(bucket.Name, versions.First());

        Directory.Delete(otherVersions.Count == 0
            ? pathRetriever.GetAbsoluteDirectoryPath(objectDirectories.ObjectDirectory)
            : pathRetriever.GetAbsoluteDirectoryPath(objectDirectories.ObjectVersionDirectory), true);

        return new ObjectServiceStatusResponse
        {
            Status = "Success"
        };
    }
}
