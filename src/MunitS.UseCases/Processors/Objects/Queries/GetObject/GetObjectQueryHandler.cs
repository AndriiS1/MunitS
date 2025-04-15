using Grpc.Core;
using MediatR;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByIdRepository;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByFileKeyRepository;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByUploadIdRepository;
using MunitS.Protos;
using MunitS.UseCases.Processors.Objects.Mappers;
namespace MunitS.UseCases.Processors.Objects.Queries.GetObject;

public class UploadFileCommandHandler(IBucketByIdRepository bucketByIdRepository,
    IObjectByFileKeyRepository objectByFileKeyRepository,
    IObjectByUploadIdRepository objectByUploadIdRepository)
    : IRequestHandler<GetObjectQuery, GetObjectResponse>
{
    public async Task<GetObjectResponse> Handle(GetObjectQuery query, CancellationToken cancellationToken)
    {
        var bucket = await bucketByIdRepository.Get(Guid.Parse(query.Request.BucketId));

        if (bucket == null) throw new RpcException(new Status(StatusCode.NotFound, $"Bucket with name: {query.Request.BucketId} is not found."));

        var objectByFileKey = await objectByFileKeyRepository.Get(bucket.Id, query.Request.FileKey);

        if (objectByFileKey == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Object with file key: {query.Request.FileKey} is not found."));
        }

        var objectVersions = await objectByUploadIdRepository.GetAll(bucket.Id, objectByFileKey.Id);

        return ObjectResponseMappers.FormatObjectResponse(objectByFileKey, objectVersions);
    }
}
