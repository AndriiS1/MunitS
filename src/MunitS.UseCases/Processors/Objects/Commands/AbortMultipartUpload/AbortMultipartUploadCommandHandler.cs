using Grpc.Core;
using MediatR;
using MunitS.Domain.Object.ObjectByFileKey;
using MunitS.Domain.Rules;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByIdRepository;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByFileKeyRepository;
using MunitS.Protos;
using MunitS.UseCases.Processors.Objects.Services;
using MunitS.UseCases.Processors.Objects.Services.MetadataBuilder;
using MunitS.UseCases.Processors.Objects.Services.ObjectBuilder;
namespace MunitS.UseCases.Processors.Objects.Commands.AbortMultipartUpload;

public class AbortMultipartUploadCommandHandler(IObjectByFileKeyRepository objectByFileKeyRepository,
    IBucketByIdRepository bucketByIdRepository,
    IObjectsBuilder objectsBuilder, IMetadataBuilder metadataBuilder) : IRequestHandler<AbortMultipartUploadCommand, ObjectServiceStatusResponse>
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

        await Task.WhenAll([
            objectsBuilder
                .ToDelete(new ObjectsBuilder.DeleteObjectByFileKey(bucket.Id, command.Request.FileKey, objectVersionToAbort.VersionId))
                .ToDelete(new ObjectsBuilder.DeleteObjectByParentPrefix(bucket.Id, FileKeyRule.GetParentPrefix(command.Request.FileKey)))
                .Build(),
            metadataBuilder
                .ToDelete(new MetadataBuilder.DeleteMetadataByObjectId(bucket.Id, objectVersionToAbort.Id, objectVersionToAbort.VersionId))
                .Build()
        ]);

        return new ObjectServiceStatusResponse
        {
            Status = "Success"
        };
    }
}
