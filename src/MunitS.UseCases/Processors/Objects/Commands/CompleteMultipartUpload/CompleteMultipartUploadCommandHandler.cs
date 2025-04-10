using Grpc.Core;
using MediatR;
using MunitS.Domain.Directory;
using MunitS.Domain.Object.ObjectByBucketId;
using MunitS.Domain.Part.PartByUploadId;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByIdRepository;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByBucketIdRepository;
using MunitS.Infrastructure.Data.Repositories.Part.PartByUploadId;
using MunitS.Protos;
using MunitS.UseCases.Processors.Service.PathRetriever.Dtos;
namespace MunitS.UseCases.Processors.Objects.Commands.CompleteMultipartUpload;

public class CompleteMultipartUploadCommandHandler(IObjectByBucketIdRepository objectByBucketIdRepository,
    IBucketByIdRepository bucketByIdRepository,
    IPartByUploadIdRepository partByUploadIdRepository) : IRequestHandler<CompleteMultipartUploadCommand, ObjectServiceStatusResponse>
{
    public async Task<ObjectServiceStatusResponse> Handle(CompleteMultipartUploadCommand command, CancellationToken cancellationToken)
    {
        var bucket = await bucketByIdRepository.Get(Guid.Parse(command.Request.BucketId));

        if (bucket == null) throw new RpcException(new Status(StatusCode.NotFound, $"Bucket with name: {command.Request.BucketId} is not found."));

        var uploadId = Guid.Parse(command.Request.UploadId);

        var objectToComplete = await objectByBucketIdRepository.GetByUploadId(bucket.Id, uploadId);

        if (objectToComplete is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "There is no instantiated object."));
        }

        if (Enum.Parse<UploadStatus>(objectToComplete.UploadStatus) == UploadStatus.UploadCompleted)
        {
            throw new RpcException(new Status(StatusCode.Aborted, "Object upload is already completed."));
        }

        var parts = await partByUploadIdRepository.GetAll(bucket.Id, uploadId);

        if (!ValidateETags(parts, command.Request.ETags.ToDictionary(kv => kv.Key, kv => kv.Value)))
        {
            throw new RpcException(new Status(StatusCode.Aborted, "Invalid eTags."));
        }

        var objectDirectories = new ObjectDirectories(bucket.Name, objectToComplete);
        var objectVersionPath = new ObjectVersionPath(objectDirectories.ObjectVersionDirectory, objectToComplete.Extension);
            
        await using var finalFile = File.Create(objectVersionPath.Value);
        
        foreach (var part in parts.OrderBy(p => p.Number))
        {
            var partPath = new PartPath(objectDirectories.TempObjectVersionDirectory, part.Number);
            
            await using var partStream = File.OpenRead(partPath.Value);
            await partStream.CopyToAsync(finalFile, cancellationToken);
        }
        
        await objectByBucketIdRepository.UpdateUploadStatus(bucket.Id, uploadId, UploadStatus.UploadCompleted);
        await partByUploadIdRepository.Delete(bucket.Id, uploadId);
        
        return new ObjectServiceStatusResponse
        {
            Status = "Success"
        };
    }

    private static bool ValidateETags(List<PartByUploadId> parts, Dictionary<int, string> eTags)
    {
        if (parts.Count != eTags.Count) return false;

        foreach (var part in parts)
        {
            var tryGetEtag = eTags.GetValueOrDefault(part.Number);

            if (tryGetEtag == null) return false;

            if (tryGetEtag != part.ETag) return false;
        }

        return true;
    }
}
