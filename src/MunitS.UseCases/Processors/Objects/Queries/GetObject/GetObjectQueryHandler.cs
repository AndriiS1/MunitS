using Grpc.Core;
using MediatR;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByNameRepository;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByFileKeyRepository;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByUploadIdRepository;
using MunitS.Protos;
using MunitS.UseCases.Processors.Objects.Mappers;
namespace MunitS.UseCases.Processors.Objects.Queries.GetObject;

public class UploadFileCommandHandler(IBucketByNameRepository bucketByNameRepository,
    IObjectByFileKeyRepository objectByFileKeyRepository,
    IObjectByUploadIdRepository objectByUploadIdRepository)
    : IRequestHandler<GetObjectQuery, GetObjectResponse>
{
    public async Task<GetObjectResponse> Handle(GetObjectQuery query, CancellationToken cancellationToken)
    {
        var bucket = await bucketByNameRepository.Get(query.Request.BucketName);

        if (bucket == null) throw new RpcException(new Status(StatusCode.NotFound, $"Bucket with name: {query.Request.BucketName} is not found."));

        var objectVersions = await objectByUploadIdRepository.GetAll(bucket.Id, Guid.Parse(query.Request.ObjectId));
        
        if(objectVersions.Count == 0) throw new RpcException(new Status(StatusCode.NotFound, $"Any version of object with id: {query.Request.ObjectId} is not found."));
        
        var objectByFileKey = await objectByFileKeyRepository.Get(bucket.Id, objectVersions.First().FileKey);

        if (objectByFileKey == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Object with file key: {objectVersions.First().FileKey} is not found."));
        }

        return ObjectResponseMappers.FormatObjectResponse(objectByFileKey, objectVersions);
    }
}
