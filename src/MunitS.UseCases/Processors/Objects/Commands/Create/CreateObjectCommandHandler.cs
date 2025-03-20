using Grpc.Core;
using MediatR;
using MunitS.Domain.Division.DivisionByBucketId;
using MunitS.Domain.Object.ObjectByFileKey;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByIdRepository;
using MunitS.Infrastructure.Data.Repositories.Division;
using MunitS.Infrastructure.Data.Repositories.Object;
using MunitS.Protos;
using MunitS.UseCases.Processors.Service.PathRetriever;
namespace MunitS.UseCases.Processors.Objects.Commands.Create;

public class CreateObjectCommandHandler(IBucketByIdRepository bucketByIdRepository,
    IObjectRepository objectRepository, IDivisionRepository divisionRepository,
    IPathRetriever pathRetriever): IRequestHandler<CreateObjectCommand, ObjectServiceStatusResponse>
{
    public async Task<ObjectServiceStatusResponse> Handle(CreateObjectCommand command, CancellationToken cancellationToken)
    {
        var bucket = await bucketByIdRepository.Get(new Guid(command.Request.BucketId));
        
        if (bucket == null) throw new RpcException(new Status(StatusCode.NotFound, $"Bucket with name: {command.Request.BucketId} is not found."));
        
        var objectVersions = await objectRepository.GetAll(command.Request.FileKey, bucket.Id);

        var divisionType = new DivisionType(command.Request.SizeInBytes);
        var division = await divisionRepository.GetNotFull(new Guid(command.Request.BucketId), divisionType);

        if (division == null)
        {
            division = DivisionByBucketId.Create(new Guid(command.Request.BucketId), divisionType, pathRetriever.GetBucketDirectory(bucket));
            
            Directory.CreateDirectory(division.Path);  
        }
        
        var newObject = ObjectByFileKey.Create(bucket.Id, command.Request.FileKey, "new_file.txt", 
            DateTime.UtcNow, new DivisionDirectory(pathRetriever.GetBucketDirectory(bucket), division.Name, division.GetSizeType()));
        
        if (objectVersions.Count > 0)
        {
            if (bucket.VersioningEnabled)
            {
                var versionsLimitReached = objectVersions.Count >= bucket.VersionsLimit;
                
                if (versionsLimitReached)
                {
                    await objectRepository.Delete(command.Request.FileKey, bucket.Id, objectVersions.Last().VersionId);
                    Directory.Delete(objectVersions.Last().Path);
                }
            }
            else
            {
                await objectRepository.Delete(command.Request.FileKey, bucket.Id, objectVersions.Last().VersionId);
            }
        }
        
        await objectRepository.Create(newObject);
        Directory.CreateDirectory(newObject.Path);
        
        return new ObjectServiceStatusResponse { Status = "Success" };
    }
}
