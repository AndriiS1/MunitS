using Grpc.Core;
using MediatR;
using MunitS.Domain.Division;
using MunitS.Infrastructure.Data.Repositories.Bucket;
using MunitS.Infrastructure.Data.Repositories.Division;
using MunitS.Infrastructure.Data.Repositories.Object;
using MunitS.Protos;
using MunitS.UseCases.Processors.Service.PathRetriever;
using Object = MunitS.Domain.Object.Object;
namespace MunitS.UseCases.Processors.Objects.Commands.Create;

public class CreateObjectCommandHandler(IBucketRepository bucketRepository,
    IObjectRepository objectRepository, IDivisionRepository divisionRepository,
    IPathRetriever pathRetriever): IRequestHandler<CreateObjectCommand, ObjectServiceStatusResponse>
{
    public async Task<ObjectServiceStatusResponse> Handle(CreateObjectCommand command, CancellationToken cancellationToken)
    {
        var bucket = await bucketRepository.Get(command.Request.BucketName);
        
        if (bucket == null) throw new RpcException(new Status(StatusCode.NotFound, $"Bucket with name: {command.Request.BucketName} is not found."));
        
        var objectVersions = await objectRepository.GetAll(command.Request.FileKey, bucket.Id);

        var divisionType = new DivisionType(command.Request.SizeInBytes);
        var division = await divisionRepository.GetNotFull(command.Request.BucketName, divisionType);

        if (division == null)
        {
            division = Division.Create(command.Request.BucketName, divisionType, pathRetriever.GetBucketDirectory(bucket));
            
            Directory.CreateDirectory(division.DivisionPath);  
        }
        
        var newObject = Object.Create(bucket.Id, command.Request.FileKey, "new_file.txt", division.Name, 
            DateTime.UtcNow, new DivisionDirectory(pathRetriever.GetBucketDirectory(bucket), division.Name, division.Type));
        
        if (objectVersions.Count > 0)
        {
            if (bucket.VersioningEnabled)
            {
                var versionsLimitReached = objectVersions.Count >= bucket.VersionsLimit;
                
                if (versionsLimitReached)
                {
                    await objectRepository.Delete(command.Request.FileKey, bucket.Id, objectVersions.Last().VersionId);
                    Directory.Delete(objectVersions.Last().ObjectPath);
                }
            }
            else
            {
                await objectRepository.Delete(command.Request.FileKey, bucket.Id, objectVersions.Last().VersionId);
            }
        }
        
        await objectRepository.Create(newObject);
        Directory.CreateDirectory(newObject.ObjectPath);
        
        return new ObjectServiceStatusResponse { Status = "Success" };
    }
}
