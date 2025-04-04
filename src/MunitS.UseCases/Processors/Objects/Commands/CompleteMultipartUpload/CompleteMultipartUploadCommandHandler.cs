using Grpc.Core;
using MediatR;
using MunitS.Domain.Object.ObjectByFileKey;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByIdRepository;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByFileKeyRepository;
using MunitS.Protos;
namespace MunitS.UseCases.Processors.Objects.Commands.CompleteMultipartUpload;

public class CompleteMultipartUploadCommandHandler(IObjectByFileKeyRepository objectByFileKeyRepository,
    IBucketByIdRepository bucketByIdRepository) : IRequestHandler<CompleteMultipartUploadCommand, ObjectServiceStatusResponse>
{
    public async Task<ObjectServiceStatusResponse> Handle(CompleteMultipartUploadCommand command, CancellationToken cancellationToken)
    {
        var bucket = await bucketByIdRepository.Get(Guid.Parse(command.Request.BucketId));

        if (bucket == null) throw new RpcException(new Status(StatusCode.NotFound, $"Bucket with name: {command.Request.BucketId} is not found."));

        var objects = await objectByFileKeyRepository.GetAll(bucket.Id, command.Request.FileKey);

        var objectVersionToCompleteUpload = objects.FirstOrDefault(o => Enum.Parse<UploadStatus>(o.UploadStatus) == UploadStatus.Instantiated);

        if (objectVersionToCompleteUpload is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "There is no instantiated object."));
        }

        await objectByFileKeyRepository.UpdateUploadStatus(bucket.Id, command.Request.FileKey, UploadStatus.UploadCompleted);

        return new ObjectServiceStatusResponse
        {
            Status = "Success"
        };
    }
}
