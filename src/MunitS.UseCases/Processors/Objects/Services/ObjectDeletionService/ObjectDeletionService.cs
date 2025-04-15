using MunitS.Domain.Directory.Dtos;
using MunitS.Domain.Division.DivisionByBucketId;
using MunitS.Domain.Object.ObjectByBucketId;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketCounter;
using MunitS.Infrastructure.Data.Repositories.Division.DivisionCounters;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByBucketIdRepository;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByFileKeyRepository;
using MunitS.Infrastructure.Data.Repositories.ObjectSuffix.ObjectSuffixByParentPrefixRepository;
using MunitS.Infrastructure.Data.Repositories.Part.PartByUploadId;
using MunitS.UseCases.Processors.Service.PathRetriever;
namespace MunitS.UseCases.Processors.Objects.Services.ObjectDeletionService;

public class ObjectDeletionService(IBucketCounterRepository bucketCounterRepository, IDivisionCounterRepository divisionCounterRepository,
    IObjectByBucketIdRepository objectByBucketIdRepository, IObjectByFileKeyRepository objectByFileKeyRepository,
    IPartByUploadIdRepository partByUploadIdRepository, IObjectSuffixByParentPrefixRepository objectSuffixByParentPrefixRepository,
    IPathRetriever pathRetriever)
{
    public async Task DeleteObject(string bucketName, ObjectByBucketId objectByBucketId, bool deleteEntireObject = true)
    {
        await bucketCounterRepository.IncrementObjectsCount(objectByBucketId.BucketId, -1);
        await bucketCounterRepository.IncrementSizeInBytesCount(objectByBucketId.BucketId, objectByBucketId.SizeInBytes);

        await divisionCounterRepository.IncrementObjectsCount(objectByBucketId.BucketId, 
            Enum.Parse<DivisionType.SizeType>(objectByBucketId.DivisionSizeType), objectByBucketId.BucketId, -1);
        
        await objectByBucketIdRepository.Delete(objectByBucketId.BucketId, objectByBucketId.UploadId);
        await objectByFileKeyRepository.Delete(objectByBucketId.BucketId, objectByBucketId.FileKey, objectByBucketId.UploadId);

        await partByUploadIdRepository.Delete(objectByBucketId.BucketId, objectByBucketId.UploadId);
        
        await DeleteObjectPrefixesRelations(objectByBucketId.BucketId, objectByBucketId.FileKey);
        
        var objectDirectories = new ObjectVersionDirectories(bucketName, objectByBucketId);

        if (deleteEntireObject)
        {
            var absoluteObjectPath = pathRetriever.GetAbsoluteDirectoryPath(objectDirectories.ObjectDirectory);
            Directory.Delete(absoluteObjectPath, true);
        }
        else
        {
            var absoluteObjectVersionPath = pathRetriever.GetAbsoluteDirectoryPath(objectDirectories.ObjectVersionDirectory);
            Directory.Delete(absoluteObjectVersionPath);
        }
    }

    private async Task DeleteObjectPrefixesRelations(Guid bucketId, string fileKey)
    {
        var trimmedFileKey =  fileKey.Trim('/');
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

        var objectParentPrefix = parentPrefixes.Count > 0 ? parentPrefixes.Last(): "/";
        
        parentPrefixes.Add(objectParentPrefix);

        parentPrefixes.Reverse();

        await objectSuffixByParentPrefixRepository.Delete(bucketId, objectParentPrefix, fileName);
        
        foreach (var prefix in parentPrefixes)
        {
            var anyRelatedObjectSuffix = await objectSuffixByParentPrefixRepository.Any(bucketId, prefix);
            
            if(anyRelatedObjectSuffix != null) continue;

            await objectSuffixByParentPrefixRepository.Delete(bucketId, objectParentPrefix);
        }
    }
}
