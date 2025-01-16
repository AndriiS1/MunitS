namespace MunitS.Services;

using Grpc.Core;
using System.IO;
using System.Threading.Tasks;

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

