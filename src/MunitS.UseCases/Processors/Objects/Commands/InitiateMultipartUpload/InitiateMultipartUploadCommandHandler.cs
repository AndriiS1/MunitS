using Grpc.Core;
using MediatR;
using MunitS.Domain.Directory;
using MunitS.Domain.Division.DivisionByBucketId;
using MunitS.Domain.Metadata.MedataByObjectId;
using MunitS.Domain.Object.ObjectByBucketId;
using MunitS.Domain.Object.ObjectByParentPrefix;
using MunitS.Domain.Rules;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByIdRepository;
using MunitS.Infrastructure.Data.Repositories.Division;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByBucketIdRepository;
using MunitS.Protos;
using MunitS.UseCases.Processors.Objects.Services;
using MunitS.UseCases.Processors.Objects.Services.DivisionBuilder;
using MunitS.UseCases.Processors.Objects.Services.MetadataBuilder;
using MunitS.UseCases.Processors.Service.PathRetriever;
using MunitS.UseCases.Processors.Service.PathRetriever.Dtos;
namespace MunitS.UseCases.Processors.Objects.Commands.InitiateMultipartUpload;

public class InitiateMultipartUploadCommandHandler(IObjectsBuilder objectsBuilder,
    IObjectByBucketIdRepository objectByBucketIdRepository,
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

        var objects = await objectByBucketIdRepository.GetAll(bucket.Id, command.Request.FileKey);

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

        var objectByFileKey = ObjectByBucketId.Create(bucket.Id, division.Id, command.Request.FileKey,
            fileName, initiatedAt, Enum.Parse<DivisionType.SizeType>(division.Type), FileKeyRule.GetExtension(command.Request.FileKey));
        var objectByParentPrefix = ObjectByParentPrefix.Create(objectByFileKey.Id, bucket.Id, fileName, objectByFileKey.UploadId, FileKeyRule.GetParentPrefix(command.Request.FileKey), initiatedAt);
        var metadataByObjectId = MetadataByObjectId.Create(bucket.Id, objectByFileKey.VersionId, objectByFileKey.Id, command.Request.ContentType, command.Request.SizeInBytes);

        var objectDirectories = new ObjectDirectories(bucket.Name, objectByFileKey);

        var absoluteObjectVersionedTempDirectory = pathRetriever.GetAbsoluteDirectoryPath(objectDirectories.TempObjectVersionDirectory);
        Directory.CreateDirectory(absoluteObjectVersionedTempDirectory);

        await Task.WhenAll(objectsBuilder
            .ToInsert(objectByFileKey)
            .ToInsert(objectByParentPrefix)
            .Build(), metadataBuilder
            .ToInsert(metadataByObjectId)
            .Build());

        return new InitiateMultipartUploadResponse
        {
            UploadId = objectByFileKey.UploadId.ToString()
        };
    }
}
