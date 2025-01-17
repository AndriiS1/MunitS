using Grpc.Core;
using MunitS.Protos;
namespace MunitS.UseCases.Services;

public class StorageService : BlobStorage.BlobStorageBase
{
    public override async Task<UploadResponse> UploadFile(UploadRequest request, ServerCallContext context)
    {
        var filePath = Path.Combine("UploadedFiles", request.Metadata.FileName);
        try
        {
            Directory.CreateDirectory("UploadedFiles");
            await File.WriteAllBytesAsync(filePath, request.Data.ToByteArray());
        }
        catch (Exception ex)
        {
            return new UploadResponse { Status = $"Failed: {ex.Message}" };
        }
        
        return new UploadResponse { Status = "Success" };
    }
}

