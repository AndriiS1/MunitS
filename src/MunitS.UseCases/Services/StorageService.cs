using Grpc.Core;
using MunitS.Domain.Bucket;
using MunitS.Domain.Chunk;
using MunitS.Infrastructure.Data.Repositories.Bucket;
using MunitS.Infrastructure.Data.Repositories.Metadata;
using MunitS.Infrastructure.Data.Repositories.Object;
using MunitS.Protos;
using Object = MunitS.Domain.Object.Object;
namespace MunitS.UseCases.Services;

public class StorageService(IMetadataRepository metadataRepository, IBucketRepository bucketRepository, IObjectRepository objectRepository)
    : BlobStorage.BlobStorageBase
{
    public override async Task<UploadResponse> UploadFile(UploadRequest request, ServerCallContext context)
    {
        var bucket = await bucketRepository.Get(request.BucketName);
        
        if(bucket == null) throw new RpcException(new Status(StatusCode.NotFound, $"Bucket with name: {request.BucketName} is not found."));
        
        var bucketDirectory = new BucketDirectory(bucket.Name);
        
        var objectVersions = await objectRepository.GetAll(request.FileKey, bucket.Id);
        
        var newObject = Object.Create(bucket.Id, request.FileKey, "new_file.txt", DateTime.UtcNow);
        
        if (objectVersions.Count > 0)
        {
            if (bucket.VersioningEnabled)
            {
                var versionsLimitReached = objectVersions.Count >= bucket.VersionsLimit;
                
                if (versionsLimitReached)
                {
                    // TODO: delete from disk
                }
            }
            else
            {
                await objectRepository.Delete(request.FileKey, bucket.Id, objectVersions.Last().VersionId);
            }
        }
        
        await objectRepository.Create(newObject);
        
        return new UploadResponse { Status = "Success" };
    }

    private static ObjectPath CreateInitialDirectories(BucketDirectory bucketDirectory, Guid objectId, Guid versionId)
    {
        var objectDirectory = new ObjectDirectory(bucketDirectory, objectId);
        var versionedObjectDirectory = new VersionedObjectDirectory(objectDirectory, versionId);
        var objectPath = new ObjectPath(versionedObjectDirectory);

        if (!Directory.Exists(objectDirectory.Value))
        {
            Directory.CreateDirectory(objectDirectory.Value);  
        }

        Directory.CreateDirectory(versionedObjectDirectory.Value);
        Directory.CreateDirectory(objectPath.Value);

        return objectPath;
    }
}

