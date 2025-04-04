using Grpc.Core;
using MediatR;
using MunitS.Domain.Division.DivisionByBucketId;
using MunitS.Domain.Object.ObjectByFileKey;
using MunitS.Domain.Object.ObjectByParentPrefix;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByIdRepository;
using MunitS.Infrastructure.Data.Repositories.Division;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByFileKeyRepository;
using MunitS.Protos;
using MunitS.UseCases.Processors.Objects.Services;
using MunitS.UseCases.Processors.Service.PathRetriever;
namespace MunitS.UseCases.Processors.Objects.Commands.InitiateMultipartUpload;

public class InitiateMultipartUploadCommandHandler(IObjectsBuilder objectsBuilder,
    IObjectByFileKeyRepository objectByFileKeyRepository,
    IBucketByIdRepository bucketByIdRepository,
    IDivisionRepository divisionRepository,
    IPathRetriever pathRetriever) : IRequestHandler<InitiateMultipartUploadCommand, InitiateMultipartUploadResponse>
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
            division = DivisionByBucketId.Create(bucket.Id, divisionType, pathRetriever.GetBucketDirectory(bucket));

            Directory.CreateDirectory(division.Path);
        }

        var fileName = GetFileName(command.Request.FileKey);
        var initiatedAt = DateTimeOffset.UtcNow;

        var divisionDirectory = new DivisionDirectory(pathRetriever.GetBucketDirectory(bucket), division.Name, division.GetSizeType());

        var objectByFileKey = ObjectByFileKey.Create(bucket.Id, command.Request.FileKey,
            fileName, initiatedAt, divisionDirectory, GetExtension(command.Request.FileKey));

        var objectByParentPrefix = ObjectByParentPrefix.Create(objectByFileKey.Id, bucket.Id, fileName, GetParentPrefix(command.Request.FileKey), initiatedAt);

        await objectsBuilder
            .ToInsert(objectByFileKey)
            .ToInsert(objectByParentPrefix)
            .Build();

        return new InitiateMultipartUploadResponse
        {
            UploadId = objectByFileKey.UploadId.ToString()
        };
    }

    private static string GetFileName(string fileKey)
    {
        var parts = fileKey.Split('/');
        return parts[^1];
    }

    private static string GetParentPrefix(string fileKey)
    {
        var parts = fileKey.Split('/');
        return string.Join("/", parts[new Range(0, parts.Length - 1)]);
    }

    private static string GetExtension(string fileKey)
    {
        var parts = fileKey.Split('.');
        return parts[^1];
    }
}
