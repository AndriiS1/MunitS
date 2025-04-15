using MunitS.Domain.Directory.Dtos;
using MunitS.Domain.Division.DivisionByBucketId;
using MunitS.Domain.Object.ObjectByUploadId;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketCounter;
using MunitS.Infrastructure.Data.Repositories.Division.DivisionCounters;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByFileKeyRepository;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByUploadIdRepository;
using MunitS.Infrastructure.Data.Repositories.ObjectSuffix.ObjectSuffixByParentPrefixRepository;
using MunitS.Infrastructure.Data.Repositories.Part.PartByUploadId;
using MunitS.UseCases.Processors.Service.PathRetriever;
namespace MunitS.UseCases.Processors.Objects.Services.ObjectDeletionService;

public class ObjectDeletionService(IBucketCounterRepository bucketCounterRepository,
    IDivisionCounterRepository divisionCounterRepository,
    IObjectByUploadIdRepository objectByUploadIdRepository,
    IObjectByFileKeyRepository objectByFileKeyRepository,
    IPartByUploadIdRepository partByUploadIdRepository,
    IObjectSuffixByParentPrefixRepository objectSuffixByParentPrefixRepository,
    IPathRetriever pathRetriever) : IObjectDeletionService
{
    public async Task DeleteOldestObjectVersion(string bucketName, ObjectByUploadId objectByUploadId)
    {
        await bucketCounterRepository.IncrementObjectsCount(objectByUploadId.BucketId, -1);
        await bucketCounterRepository.IncrementSizeInBytesCount(objectByUploadId.BucketId, objectByUploadId.SizeInBytes);

        await divisionCounterRepository.IncrementObjectsCount(objectByUploadId.BucketId,
            Enum.Parse<DivisionType.SizeType>(objectByUploadId.DivisionSizeType), objectByUploadId.BucketId, -1);

        await objectByUploadIdRepository.Delete(objectByUploadId.BucketId, objectByUploadId.UploadId);
        await objectByFileKeyRepository.Delete(objectByUploadId.BucketId, objectByUploadId.FileKey, objectByUploadId.UploadId);

        await partByUploadIdRepository.Delete(objectByUploadId.BucketId, objectByUploadId.UploadId);

        var objectDirectories = new ObjectVersionDirectories(bucketName, objectByUploadId);

        var absoluteObjectVersionPath = pathRetriever.GetAbsoluteDirectoryPath(objectDirectories.ObjectVersionDirectory);
        Directory.Delete(absoluteObjectVersionPath, true);
    }

    private async Task DeleteObjectPrefixesRelations(Guid bucketId, string fileKey)
    {
        var trimmedFileKey = fileKey.Trim('/');
        var split = trimmedFileKey.Split("/");
        var folders = split[new Range(0, split.Length - 1)];
        var fileName = split[^1];

        List<string> parentPrefixes = [];

        var parentPrefix = "/";

        foreach (var folder in folders)
        {
            parentPrefix = Path.Combine(parentPrefix, folder + "/");
            parentPrefixes.Add(parentPrefix);
        }

        var objectParentPrefix = parentPrefixes.Count > 0 ? parentPrefixes.Last() : "/";

        parentPrefixes.Add(objectParentPrefix);

        parentPrefixes.Reverse();

        await objectSuffixByParentPrefixRepository.Delete(bucketId, objectParentPrefix, fileName);

        foreach (var prefix in parentPrefixes)
        {
            var anyRelatedObjectSuffix = await objectSuffixByParentPrefixRepository.Any(bucketId, prefix);

            if (anyRelatedObjectSuffix != null) continue;

            await objectSuffixByParentPrefixRepository.Delete(bucketId, objectParentPrefix);
        }
    }
}
