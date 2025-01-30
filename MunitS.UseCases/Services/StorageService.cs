using Grpc.Core;
using Microsoft.Extensions.Options;
using MunitS.Domain.Chunk;
using MunitS.Infrastructure.Options.Storage;
using MunitS.Protos;
namespace MunitS.UseCases.Services;

public class StorageService(IOptions<StorageOptions> storageOptions) : BlobStorage.BlobStorageBase
{
    public override async Task<UploadResponse> UploadFile(UploadRequest request, ServerCallContext context)
    {
        var filePath = new FileDirectory(storageOptions.Value.RootDirectory, request.Metadata.FileName);
        
        try
        {
            await File.WriteAllBytesAsync($"{storageOptions.Value.RootDirectory}/{request.Metadata.FileName}", request.Data.ToByteArray());
        }
        catch (Exception ex)
        {
            return new UploadResponse { Status = $"Failed: {ex.Message}" };
        }
        
        return new UploadResponse { Status = "Success" };
    }
}

