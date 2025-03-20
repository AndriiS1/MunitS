using Grpc.Core;
using MediatR;
using MunitS.Domain.Object.ObjectByFileKey;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByIdRepository;
using MunitS.Infrastructure.Data.Repositories.Object;
using MunitS.Protos;
using MunitS.UseCases.Processors.Service.Compression;
using static System.Enum;
namespace MunitS.UseCases.Processors.Objects.Commands.Upload;

public class UploadObjectCommandHandler(IBucketByIdRepository bucketByIdRepository,
    IObjectRepository objectRepository, ICompressionService compressionService): IRequestHandler<UploadObjectCommand, ObjectServiceStatusResponse>
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
                    var bucket = await bucketByIdRepository.Get(new Guid(uploadObjectRequest.BucketId));

                    if (bucket == null) throw new RpcException(new Status(StatusCode.NotFound, $"Bucket with name: {uploadObjectRequest.BucketId} is not found."));

                    var objectVersions = await objectRepository.GetAll(uploadObjectRequest.FileKey, bucket.Id);

                    if (objectVersions.Count == 0)
                    {
                        throw new RpcException(new Status(StatusCode.NotFound, $"Object is not created."));
                    }
                    
                    if (Parse<UploadStatus>(objectVersions.Last().UploadStatus) != UploadStatus.Instantiated)
                    {
                        throw new RpcException(new Status(StatusCode.NotFound, $"Object is not created."));
                    }

                    fileName = uploadObjectRequest.FileKey;
                    totalChunks = uploadObjectRequest.TotalChunks;
                    
                    fileStream = new FileStream(objectVersions.Last().GetObjectDataPath(), FileMode.Create, FileAccess.Write);
                }

                if (uploadObjectRequest.FileKey != fileName || uploadObjectRequest.TotalChunks != totalChunks)
                {
                    File.Delete(fileStream!.Name);
                    
                    throw new RpcException(new Status(StatusCode.Cancelled, $"Chuck id data was changed mid upload."));
                }
                
                var compressedChunk = compressionService.CompressChunk(uploadObjectRequest.Chunk.DataStream.ToArray());
                
                await fileStream!.WriteAsync(compressedChunk.AsMemory(0, compressedChunk.Length), cancellationToken);
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
