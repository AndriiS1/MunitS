using Grpc.Core;
using MediatR;
using MunitS.Domain.Object.ObjectByFileKey;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByIdRepository;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByFileKeyRepository;
using MunitS.Protos;
using MunitS.UseCases.Processors.Objects.Services;
namespace MunitS.UseCases.Processors.Objects.Commands.AbortMultipartUpload;

public class AbortMultipartUploadCommandHandler(IObjectByFileKeyRepository objectByFileKeyRepository,
    IBucketByIdRepository bucketByIdRepository,
    IObjectsBuilder objectsBuilder) : IRequestHandler<AbortMultipartUploadCommand, ObjectServiceStatusResponse>
{
    public async Task<ObjectServiceStatusResponse> Handle(AbortMultipartUploadCommand command, CancellationToken cancellationToken)
    {
        var bucket = await bucketByIdRepository.Get(Guid.Parse(command.Request.BucketId));

        if (bucket == null) throw new RpcException(new Status(StatusCode.NotFound, $"Bucket with name: {command.Request.BucketId} is not found."));

        var objects = await objectByFileKeyRepository.GetAll(bucket.Id, command.Request.FileKey);

        var objectVersionToAbort = objects.FirstOrDefault(o => Enum.Parse<UploadStatus>(o.UploadStatus) == UploadStatus.Instantiated);

        if (objectVersionToAbort is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "There is no instantiated object."));
        }

        await objectsBuilder
            .ToDelete(new ObjectsBuilder.DeleteObjectByFileKey(bucket.Id, command.Request.FileKey, objectVersionToAbort.VersionId))
            .ToDelete(new ObjectsBuilder.DeleteObjectByParentPrefix(bucket.Id, GetParentPrefix(command.Request.FileKey)))
            .Build();

        return new ObjectServiceStatusResponse
        {
            Status = "Success"
        };
    }

    private static string GetParentPrefix(string fileKey)
    {
        var parts = fileKey.Split('/');
        return string.Join("/", parts[new Range(0, parts.Length - 1)]);
    }
}
