using Grpc.Core;
using MediatR;
using MunitS.Domain.Directory;
using MunitS.Domain.Directory.Dtos;
using MunitS.Domain.Division.DivisionByBucketId;
using MunitS.Domain.Object.ObjectByFileKey;
using MunitS.Domain.Object.ObjectByUploadId;
using MunitS.Domain.Rules;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByIdRepository;
using MunitS.Infrastructure.Data.Repositories.Division.DivisionById;
using MunitS.Infrastructure.Data.Repositories.Division.DivisionCounters;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByFileKeyRepository;
using MunitS.Infrastructure.Data.Repositories.ObjectSuffix.ObjectSuffixByParentPrefixRepository;
using MunitS.Protos;
using MunitS.UseCases.Processors.Objects.Services.DivisionBuilder;
using MunitS.UseCases.Processors.Objects.Services.ObjectBuilder;
using MunitS.UseCases.Processors.Service.ObjectSuffixesRetriever;
using MunitS.UseCases.Processors.Service.PathRetriever;
namespace MunitS.UseCases.Processors.Objects.Commands.InitiateMultipartUpload;

public class InitiateMultipartUploadCommandHandler(IObjectsBuilder objectsBuilder,
    IObjectByFileKeyRepository objectByFileKeyRepository,
    IBucketByIdRepository bucketByIdRepository,
    IPathRetriever pathRetriever,
    IDivisionByIdRepository divisionByIdRepository,
    IDivisionBuilder divisionBuilder,
    IDivisionCounterRepository divisionCounterRepository,
    IObjectSuffixByParentPrefixRepository objectSuffixByParentPrefixRepository)
    : IRequestHandler<InitiateMultipartUploadCommand, InitiateMultipartUploadResponse>
{
    public async Task<InitiateMultipartUploadResponse> Handle(InitiateMultipartUploadCommand command, CancellationToken cancellationToken)
    {
        var bucket = await bucketByIdRepository.Get(Guid.Parse(command.Request.BucketId));

        if (bucket == null) throw new RpcException(new Status(StatusCode.NotFound, $"Bucket with name: {command.Request.BucketId} is not found."));

        var objects = await objectByFileKeyRepository.GetAll(bucket.Id, command.Request.FileKey);

        if (objects.Any(o => o.UploadStatus == UploadStatus.Instantiated.ToString()))
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Object upload is already instantiated."));
        }

        var divisionType = new DivisionType(command.Request.SizeInBytes);

        var bucketDivisions = await divisionByIdRepository.GetAll(bucket.Id, divisionType.Type);

        var divisionCounters = await divisionCounterRepository.GetAll(bucket.Id, divisionType.Type);

        var division = bucketDivisions.FirstOrDefault(d => divisionCounters
            .FirstOrDefault(c => c.Id == d.Id)?.ObjectsCount < d.ObjectsLimit);

        if (division == null)
        {
            division = DivisionByBucketId.Create(bucket.Id, divisionType);
            var divisionDirectory = new DivisionDirectory(bucket.Name, division.Id, Enum.Parse<DivisionType.SizeType>(division.Type));

            var absoluteDivisionDirectory = pathRetriever.GetAbsoluteDirectoryPath(divisionDirectory);
            Directory.CreateDirectory(absoluteDivisionDirectory);

            await divisionBuilder
                .ToInsert(division)
                .Build();
        }

        var fileName = FileKeyRule.GetFileName(command.Request.FileKey);
        var initiatedAt = DateTimeOffset.UtcNow;
        var divisionSizeType = Enum.Parse<DivisionType.SizeType>(division.Type);

        var objectByBucketId = ObjectByUploadId.Create(bucket.Id, division.Id, command.Request.FileKey,
            fileName, initiatedAt, divisionSizeType, FileKeyRule.GetExtension(command.Request.FileKey), command.Request.MimeType, command.Request.SizeInBytes);
        var objectByFileKey = ObjectByFileKey.Create(bucket.Id, objectByBucketId.Id, objectByBucketId.UploadId, command.Request.FileKey);

        var objectDirectories = new ObjectVersionDirectories(bucket.Name, objectByBucketId);

        var absoluteObjectVersionedTempDirectory = pathRetriever.GetAbsoluteDirectoryPath(objectDirectories.TempObjectVersionDirectory);

        if (!Directory.Exists(absoluteObjectVersionedTempDirectory))
        {
            Directory.CreateDirectory(absoluteObjectVersionedTempDirectory);
        }

        List<Task> tasks =
        [
            objectsBuilder
                .ToInsert(objectByBucketId)
                .ToInsert(objectByFileKey)
                .Build()
        ];

        var prefixes = ObjectSuffixesRetriever.GetObjectSuffixes(bucket.Id, command.Request.FileKey, objectByBucketId.Id, command.Request.MimeType);

        tasks.AddRange(prefixes.Select(objectSuffixByParentPrefixRepository.Create));

        await Task.WhenAll(tasks);

        return new InitiateMultipartUploadResponse
        {
            UploadId = objectByBucketId.UploadId.ToString()
        };
    }
}
