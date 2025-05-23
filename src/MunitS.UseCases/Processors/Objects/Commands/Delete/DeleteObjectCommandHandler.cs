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
namespace MunitS.UseCases.Processors.Objects.Commands.Delete;

public class DeleteObjectCommandHandler(
    IObjectByFileKeyRepository objectByFileKeyRepository,
    IBucketCounterRepository bucketCounterRepository,
    IDivisionCounterRepository divisionCounterRepository,
    IBucketByIdRepository bucketByIdRepository,
    IObjectByUploadIdRepository objectByUploadIdRepository,
    IPathRetriever pathRetriever,
    IObjectDeletionService objectDeletionService) : IRequestHandler<DeleteObjectCommand, ObjectServiceStatusResponse>
{
    public async Task<ObjectServiceStatusResponse> Handle(DeleteObjectCommand command, CancellationToken cancellationToken)
    {
        var bucket = await bucketByIdRepository.Get(Guid.Parse(command.Request.BucketId));

        if (bucket == null) throw new RpcException(new Status(StatusCode.NotFound, $"Bucket with name: {command.Request.BucketId} is not found."));

        var objectToDelete = await objectByFileKeyRepository.Get(bucket.Id, command.Request.FileKey);

        if (objectToDelete is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "No object by file key found."));
        }

        var versions = await objectByUploadIdRepository.GetAll(bucket.Id, objectToDelete.Id);

        if (versions.Count == 0)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "No object versions found."));
        }

        List<Task> tasks =
        [
            objectByUploadIdRepository.Delete(bucket.Id, objectToDelete.Id),
            objectByFileKeyRepository.Delete(bucket.Id, objectToDelete.FileKey),
            objectDeletionService.DeleteObjectPrefixesRelations(bucket.Id, objectToDelete.FileKey)
        ];

        versions.ForEach(v =>
        {
            tasks.Add(divisionCounterRepository.IncrementObjectsCount(bucket.Id, Enum.Parse<DivisionType.SizeType>(v.DivisionSizeType), v.DivisionId, -1));
        });

        tasks.Add(bucketCounterRepository.IncrementObjectsCount(bucket.Id, -1 * versions.Count));
        tasks.Add(bucketCounterRepository.IncrementSizeInBytesCount(bucket.Id, -1 * versions.Sum(v => v.SizeInBytes)));

        await Task.WhenAll(tasks);

        var objectDirectories = new ObjectVersionDirectories(bucket.Name, versions.First());

        Directory.Delete(pathRetriever.GetAbsoluteDirectoryPath(objectDirectories.ObjectDirectory), true);

        return new ObjectServiceStatusResponse
        {
            Status = "Success"
        };
    }
}
