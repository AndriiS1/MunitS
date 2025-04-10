using MunitS.Domain.Bucket.BucketById;
using MunitS.Domain.FolderPrefixes.FolderPrefixByParentPrefix;
using MunitS.Domain.Object.ObjectByParentPrefix;
using MunitS.Protos;
namespace MunitS.UseCases.Processors.Objects.Mappers;

public static class ObjectResponseMappers
{
    public static GetObjectsByPrefixResponse FormatGetBucketResponse(List<ObjectByParentPrefix> objects, List<FolderPrefixByParentPrefix> folders)
    {
        return new GetObjectsByPrefixResponse
        {
            Content = FormatObjectsResponse(objects, folders)
        };
    }

    private static BucketResponse FormatBucketResponse(BucketById bucketById)
    {
        return new BucketResponse
        {
            Id = bucketById.Id.ToString(),
            Name = bucketById.Name,
            ObjectsCount = bucketById.ObjectsCount,
            Size = bucketById.SizeInBytes,
            VersioningEnabled = bucketById.VersioningEnabled,
            VersionsLimit = bucketById.VersionsLimit,
        };
    }
    
    private static ObjectByPrefixResponse FormatObjectByPrefixResponse(ObjectByParentPrefix @object)
    {
        return new ObjectByPrefixResponse
        {
            Id = @object.Id.ToString(),
            FileName = @object.FileName,
            UploadedAt = @object.UploadedAt.ToString(),
        };
    }
    
    private static FolderByPrefixResponse FormatFolderByPrefixResponse(FolderPrefixByParentPrefix folder)
    {
        return new FolderByPrefixResponse
        {
            Prefix = folder.Prefix,
        };
    }

    
    private static ObjectsResponse FormatObjectsResponse(List<ObjectByParentPrefix> objects, List<FolderPrefixByParentPrefix> folders)
    {
        var response = new ObjectsResponse();
        response.Objects.AddRange(objects.Select(FormatObjectByPrefixResponse));
        response.Folders.AddRange(folders.Select(FormatFolderByPrefixResponse));
        
        return response;
    }
    
}
