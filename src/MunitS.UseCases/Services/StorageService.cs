using Grpc.Core;
using MediatR;
using MunitS.Domain.Bucket;
using MunitS.Domain.Chunk;
using MunitS.Domain.Versioning;
using MunitS.Infrastructure.Data.Repositories.Bucket;
using MunitS.Infrastructure.Data.Repositories.Metadata;
using MunitS.Infrastructure.Data.Repositories.Object;
using MunitS.Infrastructure.Data.Repositories.ObjectVersioning;
using MunitS.Protos;
using Metadata = MunitS.Domain.Metadata.Metadata;
using Object = MunitS.Domain.Object.Object;
namespace MunitS.UseCases.Services;

public class StorageService(Mediator mediator, IMetadataRepository metadataRepository, IBucketRepository bucketRepository, IObjectRepository objectRepository,
    IObjectVersionRepository objectVersionRepository) : BlobStorage.BlobStorageBase
{
    public override async Task<UploadResponse> UploadFile(UploadRequest request, ServerCallContext context)
    {
        var bucket = await bucketRepository.Get(request.BucketName);
        
        if(bucket == null) throw new RpcException(new Status(StatusCode.NotFound, $"Bucket with name: {request.BucketName} is not found."));
        
        var bucketDirectory = new BucketDirectory(bucket.Name);
        
        var @object = await objectRepository.Get(bucket.Id, request.FileKey);
        
        var uploaded = DateTime.UtcNow;
        

        if (@object is null)
        {
            var newObject = new Object(bucket.Id);
            var newVersion = new ObjectVersion(newObject.Id, uploaded);

            await objectVersionRepository.Create(newVersion);
            
            await objectRepository.Create(newObject);
            
            var objectPath = CreateInitialDirectories(bucketDirectory, newObject.Id, newVersion.Id);
            
            //TODO: file chuncking
        }
        else
        {
            var newVersion = new ObjectVersion(@object.Id, uploaded);
            
            var objectVersions = await objectVersionRepository.GetAll(@object.Id);
                
            var versionsLimitReached = objectVersions.Count >= bucket.VersionsLimit;
            
            if (objectVersions.Count == 0) throw new RpcException(new Status(StatusCode.NotFound, $"No versions found for object: {@object.Id}."));
            
            await Task.WhenAll(objectVersionRepository.DeleteOldest(@object.Id), objectVersionRepository.Create(newVersion));
                    
            //TODO: delete oldest version from disk
            if (bucket.VersioningEnabled)
            {
                if (versionsLimitReached)
                {
                }
            }

            var objectPath = CreateInitialDirectories(bucketDirectory, @object.Id, newVersion.Id);

            //TODO: file chuncking
        }
        
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

