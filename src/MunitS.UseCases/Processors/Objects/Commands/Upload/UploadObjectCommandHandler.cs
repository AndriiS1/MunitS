using Grpc.Core;
using MediatR;
using MunitS.Domain.Object;
using MunitS.Infrastructure.Data.Repositories.Bucket;
using MunitS.Infrastructure.Data.Repositories.Object;
using MunitS.Protos;
using static System.Enum;
namespace MunitS.UseCases.Processors.Objects.Commands.Upload;

public class UploadObjectCommandHandler(IBucketRepository bucketRepository,
    IObjectRepository objectRepository): IRequestHandler<UploadObjectCommand, ObjectServiceStatusResponse>
{
    public async Task<ObjectServiceStatusResponse> Handle(UploadObjectCommand command, CancellationToken cancellationToken)
    {
        long totalChunks = 0;
        var currentChunkIndex = 0;
        var fileName = string.Empty;
        FileStream? fileStream = null;

        try
        {
            await foreach (var uploadObjectRequest in command.RequestStream.ReadAllAsync(cancellationToken: cancellationToken))
            {
                if (currentChunkIndex == 0)
                {
                    var bucket = await bucketRepository.Get(uploadObjectRequest.BucketName);

                    if (bucket == null) throw new RpcException(new Status(StatusCode.NotFound, $"Bucket with name: {uploadObjectRequest.BucketName} is not found."));

                    var objectVersions = await objectRepository.GetAll(uploadObjectRequest.FileKey, bucket.Id);

                    if (objectVersions.Count == 0)
                    {
                        throw new RpcException(new Status(StatusCode.NotFound, $"Object is not created."));
                    }

                    var uploadStatusParseSucceeded = TryParse<UploadStatus>(objectVersions.Last().UploadStatus, out var uploadStatus);

                    if (!uploadStatusParseSucceeded || uploadStatus != UploadStatus.Instantiated)
                    {
                        throw new RpcException(new Status(StatusCode.NotFound, $"Object is not created."));
                    }

                    fileName = uploadObjectRequest.FileKey;
                    totalChunks = uploadObjectRequest.TotalChunks;
                    
                    fileStream = new FileStream($"{objectVersions.Last().Path}/data.txt", FileMode.Create, FileAccess.Write);
                }

                if (uploadObjectRequest.FileKey != fileName || uploadObjectRequest.TotalChunks != totalChunks)
                {
                    //TODO: abort
                }
                
                await fileStream!.WriteAsync(uploadObjectRequest.Chunk.DataStream.ToArray().AsMemory(0, uploadObjectRequest.Chunk.DataStream.Length), cancellationToken);
                currentChunkIndex++;
            }

            fileStream!.Seek(0, SeekOrigin.Begin);
        }
        finally
        {
            if (fileStream != null) await fileStream.DisposeAsync();
        }
        
        return new ObjectServiceStatusResponse {Status = "Success"};
    }
}
