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

        var objectByFileKey = await objectByFileKeyRepository.Get(bucket.Id, query.Request.FileKey);

        if (objectByFileKey == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Object with file key: {query.Request.FileKey} is not found."));
        }

        var objectVersions = await objectByUploadIdRepository.GetAll(bucket.Id, objectByFileKey.Id);

        return ObjectResponseMappers.FormatObjectResponse(objectByFileKey, objectVersions);
    }
}
