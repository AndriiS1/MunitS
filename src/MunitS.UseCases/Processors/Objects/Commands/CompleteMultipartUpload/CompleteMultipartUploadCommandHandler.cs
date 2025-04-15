using Grpc.Core;
using MediatR;
using MunitS.Domain.Directory;
using MunitS.Domain.Directory.Dtos;
using MunitS.Domain.Division.DivisionByBucketId;
using MunitS.Domain.Object.ObjectByBucketId;
using MunitS.Domain.Part.PartByUploadId;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByIdRepository;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketCounter;
using MunitS.Infrastructure.Data.Repositories.Division.DivisionCounters;
using MunitS.Infrastructure.Data.Repositories.Metadata;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByBucketIdRepository;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByFileKeyRepository;
using MunitS.Infrastructure.Data.Repositories.ObjectSuffix.ObjectSuffixByParentPrefixRepository;
using MunitS.Infrastructure.Data.Repositories.Part.PartByUploadId;
using MunitS.Protos;
using MunitS.UseCases.Processors.Service.PathRetriever;
namespace MunitS.UseCases.Processors.Objects.Commands.CompleteMultipartUpload;

public class CompleteMultipartUploadCommandHandler(IObjectByBucketIdRepository objectByBucketIdRepository,
    IBucketByIdRepository bucketByIdRepository,
    IPathRetriever pathRetriever,
    IMetadataByObjectIdRepository metadataByObjectIdRepository,
    IPartByUploadIdRepository partByUploadIdRepository,
    IDivisionCounterRepository divisionCounterRepository,
    IBucketCounterRepository bucketCounterRepository,
    IObjectByFileKeyRepository objectByFileKeyRepository,
    IObjectSuffixByParentPrefixRepository objectSuffixByParentPrefixRepository) : IRequestHandler<CompleteMultipartUploadCommand, ObjectServiceStatusResponse>
{
    public async Task<ObjectServiceStatusResponse> Handle(CompleteMultipartUploadCommand command, CancellationToken cancellationToken)
    {
        var bucket = await bucketByIdRepository.Get(Guid.Parse(command.Request.BucketId));

        if (bucket == null) throw new RpcException(new Status(StatusCode.NotFound, $"Bucket with name: {command.Request.BucketId} is not found."));

        var uploadId = Guid.Parse(command.Request.UploadId);

        var objectToComplete = await objectByBucketIdRepository.GetByUploadId(bucket.Id, uploadId);

        if (objectToComplete is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "There is no instantiated object."));
        }

        var metadata = await metadataByObjectIdRepository.Get(bucket.Id, uploadId);

        if (metadata is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Cannot find metadata for object version."));
        }

        if (objectToComplete.UploadStatus == UploadStatus.Completed.ToString())
        {
            throw new RpcException(new Status(StatusCode.Aborted, "Object upload is already completed."));
        }

        var parts = await partByUploadIdRepository.GetAll(bucket.Id, uploadId);

        if (!ValidateETags(parts, command.Request.ETags.ToDictionary(kv => kv.Key, kv => kv.Value)))
        {
            throw new RpcException(new Status(StatusCode.Aborted, "Invalid eTags."));
        }

        var objectDirectories = new ObjectVersionDirectories(bucket.Name, objectToComplete);
        var objectVersionPath = new ObjectVersionPath(objectDirectories.ObjectVersionDirectory, objectToComplete.Extension);
        var absoluteObjectVersionPath = pathRetriever.GetAbsoluteDirectoryPath(objectVersionPath);

        await using var finalFile = File.Create(absoluteObjectVersionPath);

        foreach (var part in parts.OrderBy(p => p.Number))
        {
            var absolutePartPath = pathRetriever.GetAbsoluteDirectoryPath(new PartPath(objectDirectories.TempObjectVersionDirectory, part.Number));

            await using var partStream = File.OpenRead(absolutePartPath);
            await partStream.CopyToAsync(finalFile, cancellationToken);
        }

        List<Task> tasks =
        [
            objectByBucketIdRepository.UpdateUploadStatus(bucket.Id, uploadId, UploadStatus.Completed),
            objectByFileKeyRepository.UpdateUploadStatus(bucket.Id, objectToComplete.FileKey, uploadId, UploadStatus.Completed),
            partByUploadIdRepository.Delete(bucket.Id, uploadId),
            bucketCounterRepository.IncrementObjectsCount(bucket.Id),
            bucketCounterRepository.IncrementSizeInBytesCount(bucket.Id, metadata.SizeInBytes),
            divisionCounterRepository.IncrementObjectsCount(bucket.Id, Enum.Parse<DivisionType.SizeType>(objectToComplete.DivisionSizeType), objectToComplete.DivisionId)
        ];

        await Task.WhenAll(tasks);

        Directory.Delete(pathRetriever.GetAbsoluteDirectoryPath(objectDirectories.TempObjectVersionDirectory), true);

        return new ObjectServiceStatusResponse
        {
            Status = "Success"
        };
    }

    private static bool ValidateETags(List<PartByUploadId> parts, Dictionary<int, string> eTags)
    {
        if (parts.Count != eTags.Count) return false;

        foreach (var part in parts)
        {
            var tryGetEtag = eTags.GetValueOrDefault(part.Number);

            if (tryGetEtag == null) return false;

            if (tryGetEtag != part.ETag) return false;
        }

        return true;
    }
}
