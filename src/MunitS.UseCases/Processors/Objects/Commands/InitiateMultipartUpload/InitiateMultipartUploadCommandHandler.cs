using Grpc.Core;
using MediatR;
using MunitS.Domain.Directory;
using MunitS.Domain.Division.DivisionByBucketId;
using MunitS.Domain.Metadata.MedataByObjectId;
using MunitS.Domain.Object.ObjectByFileKey;
using MunitS.Domain.Object.ObjectByParentPrefix;
using MunitS.Domain.Rules;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByIdRepository;
using MunitS.Infrastructure.Data.Repositories.Division;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByFileKeyRepository;
using MunitS.Protos;
using MunitS.UseCases.Processors.Objects.Services;
using MunitS.UseCases.Processors.Objects.Services.MetadataBuilder;
namespace MunitS.UseCases.Processors.Objects.Commands.InitiateMultipartUpload;

public class InitiateMultipartUploadCommandHandler(IObjectsBuilder objectsBuilder,
    IObjectByFileKeyRepository objectByFileKeyRepository,
    IBucketByIdRepository bucketByIdRepository,
    IDivisionRepository divisionRepository, IMetadataBuilder metadataBuilder) : IRequestHandler<InitiateMultipartUploadCommand, InitiateMultipartUploadResponse>
{
    public async Task<InitiateMultipartUploadResponse> Handle(InitiateMultipartUploadCommand command, CancellationToken cancellationToken)
    {
        var bucket = await bucketByIdRepository.Get(Guid.Parse(command.Request.BucketId));

        if (bucket == null) throw new RpcException(new Status(StatusCode.NotFound, $"Bucket with name: {command.Request.BucketId} is not found."));

        var objects = await objectByFileKeyRepository.GetAll(bucket.Id, command.Request.FileKey);

        if (objects.Any(o => Enum.Parse<UploadStatus>(o.UploadStatus) == UploadStatus.Instantiated))
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Object upload is already instantiated."));
        }

        var divisionType = new DivisionType(command.Request.SizeInBytes);
        var division = await divisionRepository.GetNotFull(Guid.Parse(command.Request.BucketId), divisionType);

        if (division == null)
        {
            division = DivisionByBucketId.Create(bucket.Id, bucket.Name, divisionType);

            Directory.CreateDirectory(division.Path);
        }

        var fileName = FileKeyRule.GetFileName(command.Request.FileKey);
        var initiatedAt = DateTimeOffset.UtcNow;

        var divisionDirectory = new DivisionDirectory(bucket.Name, division.Id, division.GetSizeType());

        var objectByFileKey = ObjectByFileKey.Create(bucket.Id, command.Request.FileKey,
            fileName, initiatedAt, divisionDirectory, FileKeyRule.GetExtension(command.Request.FileKey));

        var objectByParentPrefix = ObjectByParentPrefix.Create(objectByFileKey.Id, bucket.Id, fileName, FileKeyRule.GetParentPrefix(command.Request.FileKey), initiatedAt);

        var metadataByObjectId = MetadataByObjectId.Create(bucket.Id, objectByFileKey.VersionId, objectByFileKey.Id, command.Request.ContentType, command.Request.SizeInBytes);
        
        Directory.CreateDirectory(objectByFileKey.GetObjectTempPath());

        await Task.WhenAll([
            objectsBuilder
                .ToInsert(objectByFileKey)
                .ToInsert(objectByParentPrefix)
                .Build(),
            metadataBuilder
                .ToInsert(metadataByObjectId)
                .Build()
        ]);

        return new InitiateMultipartUploadResponse
        {
            UploadId = objectByFileKey.UploadId.ToString()
        };
    }
}
