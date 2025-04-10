using Grpc.Core;
using MediatR;
using MunitS.Domain.Directory;
using MunitS.Domain.Division.DivisionByBucketId;
using MunitS.Domain.Metadata.MedataByObjectId;
using MunitS.Domain.Object.ObjectByBucketId;
using MunitS.Domain.Object.ObjectByFileKey;
using MunitS.Domain.Object.ObjectByParentPrefix;
using MunitS.Domain.Rules;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByIdRepository;
using MunitS.Infrastructure.Data.Repositories.Division;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByFileKeyRepository;
using MunitS.Protos;
using MunitS.UseCases.Processors.Objects.Services.DivisionBuilder;
using MunitS.UseCases.Processors.Objects.Services.MetadataBuilder;
using MunitS.UseCases.Processors.Objects.Services.ObjectBuilder;
using MunitS.UseCases.Processors.Service.PathRetriever;
using MunitS.UseCases.Processors.Service.PathRetriever.Dtos;
namespace MunitS.UseCases.Processors.Objects.Commands.InitiateMultipartUpload;

public class InitiateMultipartUploadCommandHandler(IObjectsBuilder objectsBuilder,
    IObjectByFileKeyRepository objectByFileKeyRepository,
    IBucketByIdRepository bucketByIdRepository,
    IPathRetriever pathRetriever,
    IDivisionRepository divisionRepository,
    IDivisionBuilder divisionBuilder,
    IMetadataBuilder metadataBuilder) : IRequestHandler<InitiateMultipartUploadCommand, InitiateMultipartUploadResponse>
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
            var divisionDirectory = new DivisionDirectory(bucket.Name, division.Id, division.GetSizeType());

            var absoluteDivisionDirectory = pathRetriever.GetAbsoluteDirectoryPath(divisionDirectory);
            Directory.CreateDirectory(absoluteDivisionDirectory);

            await divisionBuilder
                .ToInsert(division)
                .Build();
        }

        var fileName = FileKeyRule.GetFileName(command.Request.FileKey);
        var initiatedAt = DateTimeOffset.UtcNow;

        var objectByBucketId = ObjectByBucketId.Create(bucket.Id, division.Id, command.Request.FileKey,
            fileName, initiatedAt, Enum.Parse<DivisionType.SizeType>(division.Type), FileKeyRule.GetExtension(command.Request.FileKey));
        var objectByParentPrefix = ObjectByParentPrefix.Create(objectByBucketId.Id, bucket.Id, fileName, objectByBucketId.UploadId, FileKeyRule.GetParentPrefix(command.Request.FileKey), initiatedAt);
        var objectByFileKey = ObjectByFileKey.Create(bucket.Id, objectByBucketId.Id, objectByBucketId.UploadId, command.Request.FileKey);
        var metadataByObjectId = MetadataByObjectId.Create(bucket.Id, objectByBucketId.UploadId, objectByBucketId.Id, command.Request.ContentType, command.Request.SizeInBytes);

        var objectDirectories = new ObjectDirectories(bucket.Name, objectByBucketId);

        var absoluteObjectVersionedTempDirectory = pathRetriever.GetAbsoluteDirectoryPath(objectDirectories.TempObjectVersionDirectory);
        Directory.CreateDirectory(absoluteObjectVersionedTempDirectory);

        await Task.WhenAll(objectsBuilder
            .ToInsert(objectByBucketId)
            .ToInsert(objectByParentPrefix)
            .ToInsert(objectByFileKey)
            .Build(), metadataBuilder
            .ToInsert(metadataByObjectId)
            .Build());

        return new InitiateMultipartUploadResponse
        {
            UploadId = objectByBucketId.UploadId.ToString()
        };
    }
}
