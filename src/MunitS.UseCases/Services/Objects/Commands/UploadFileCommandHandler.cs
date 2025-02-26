using Grpc.Core;
using MediatR;
using MunitS.Domain.Bucket;
using MunitS.Domain.Chunk;
using MunitS.Infrastructure.Data.Repositories.Bucket;
using MunitS.Infrastructure.Data.Repositories.Object;
using MunitS.Protos;
using Object = MunitS.Domain.Object.Object;
namespace MunitS.UseCases.Services.Objects.Commands;

public class UploadFileCommandHandler(IBucketRepository bucketRepository,
    IObjectRepository objectRepository): IRequestHandler<UploadFileCommand, ObjectServiceStatusResponse>
{
    public async Task<ObjectServiceStatusResponse> Handle(UploadFileCommand command, CancellationToken cancellationToken)
    {
        var bucket = await bucketRepository.Get(command.Request.BucketName);
        
        if(bucket == null) throw new RpcException(new Status(StatusCode.NotFound, $"Bucket with name: {command.Request.BucketName} is not found."));
        
        var bucketDirectory = new BucketDirectory(bucket.Name);
        
        var objectVersions = await objectRepository.GetAll(command.Request.FileKey, bucket.Id);
        
        var newObject = Object.Create(bucket.Id, command.Request.FileKey, "new_file.txt", DateTime.UtcNow);
        
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
                await objectRepository.Delete(command.Request.FileKey, bucket.Id, objectVersions.Last().VersionId);
            }
        }
        
        await objectRepository.Create(newObject);
        
        return new ObjectServiceStatusResponse { Status = "Success" };
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
