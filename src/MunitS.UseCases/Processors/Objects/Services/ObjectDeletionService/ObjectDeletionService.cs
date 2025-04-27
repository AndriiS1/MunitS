using MunitS.Domain.Directory.Dtos;
using MunitS.Domain.Division.DivisionByBucketId;
using MunitS.Domain.Object.ObjectByUploadId;
using MunitS.Domain.ObjectSuffix.ObjectSuffixByParentPrefix;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketCounter;
using MunitS.Infrastructure.Data.Repositories.Division.DivisionCounters;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByUploadIdRepository;
using MunitS.Infrastructure.Data.Repositories.ObjectSuffix.ObjectSuffixByParentPrefixRepository;
using MunitS.Infrastructure.Data.Repositories.Part.PartByUploadId;
using MunitS.UseCases.Processors.Service.PathRetriever;
namespace MunitS.UseCases.Processors.Objects.Services.ObjectDeletionService;

public class ObjectDeletionService(IBucketCounterRepository bucketCounterRepository,
    IDivisionCounterRepository divisionCounterRepository,
    IObjectByUploadIdRepository objectByUploadIdRepository,
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

        await partByUploadIdRepository.Delete(objectByUploadId.BucketId, objectByUploadId.UploadId);

        var objectDirectories = new ObjectVersionDirectories(bucketName, objectByUploadId);

        var absoluteObjectVersionPath = pathRetriever.GetAbsoluteDirectoryPath(objectDirectories.ObjectVersionDirectory);
        Directory.Delete(absoluteObjectVersionPath, true);
    }

    public async Task DeleteObjectPrefixesRelations(Guid bucketId, string fileKey)
    {
        var trimmedFileKey = fileKey.Trim('/');
        var split = trimmedFileKey.Split("/");
        var folders = split[new Range(0, split.Length - 1)];
        var fileName = split[^1];

        List<string> folderPrefixes = [];

        var parentPrefix = "/";

        foreach (var folder in folders)
        {
            parentPrefix = Path.Combine(parentPrefix, folder + "/");
            folderPrefixes.Add(parentPrefix);
        }

        var objectParentPrefix = folderPrefixes.Count > 0 ? folderPrefixes.Last() : "/";

        folderPrefixes.Reverse();

        await objectSuffixByParentPrefixRepository.Delete(bucketId, objectParentPrefix, PrefixType.Object, fileName);

        foreach (var folderPrefix in folderPrefixes)
        {
            var anyRelatedObjectSuffix = (await objectSuffixByParentPrefixRepository.FetchTwoAsync(bucketId, folderPrefix)).FirstOrDefault(p => p.Suffix != fileName);

            if (anyRelatedObjectSuffix != null) continue;

            await objectSuffixByParentPrefixRepository.Delete(bucketId, GetFolderPrefix(folderPrefix), PrefixType.Directory, GetFolderSuffix(folderPrefix));
        }
    }

    private static string GetFolderSuffix(string folderPrefix)
    {
        var split = folderPrefix.Trim('/').Split("/");
        
        return split.Last();
    }
    
    private static string GetFolderPrefix(string folderPrefix)
    {
        var split = folderPrefix.Trim('/').Split("/");
        var prevFolders = split[new Range(0, split.Length - 1)];

        return prevFolders.Length > 0 ? $"/{string.Join("/", prevFolders)}/" : "/";
    }
}
