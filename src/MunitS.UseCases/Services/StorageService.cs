using Grpc.Core;
using Microsoft.Extensions.Options;
using MunitS.Domain.Chunk;
using MunitS.Domain.Metadata;
using MunitS.Domain.Versioning;
using MunitS.Domain.Versioning.Configs;
using MunitS.Infrastructure.Data.Repositories.Metadata;
using MunitS.Infrastructure.Options.Storage;
using MunitS.Protos;
using Metadata = MunitS.Domain.Metadata.Metadata;
namespace MunitS.UseCases.Services;

public class StorageService(IOptions<StorageOptions> storageOptions, IMetadataRepository metadataRepository) : BlobStorage.BlobStorageBase
{
    public override async Task<UploadResponse> UploadFile(UploadRequest request, ServerCallContext context)
    {
        var fileGuid = Guid.NewGuid();
        
        var fileDirectory = new ObjectDirectory(fileGuid);
        
        if (!DirectoryExistsOrNotEmpty(fileDirectory.Value))
        {
            Directory.CreateDirectory(fileDirectory.Value);
            
            var objectVersionGuid  = Guid.NewGuid();
            
            var versioning = new Versioning(fileDirectory, new DisabledVersioning(objectVersionGuid), request.Metadata.FileKey);
            
            await versioning.Write();

            var fileVersionedDirectory = new FileVersionedDirectory(fileDirectory, objectVersionGuid);
            
            var metadata = new Metadata(fileVersionedDirectory, 5, objectVersionGuid, request.Metadata.FileKey, DateTime.UtcNow);
            
            await metadataRepository.Create(metadata);

            // Directory.CreateDirectory(fileVersionedDirectory.Value);
            //
            // await metadata.Write();
            //
            // var filePath = new ObjectPath(fileVersionedDirectory, request.Metadata.FileKey);
            //
            // try
            // {
            //     await File.WriteAllBytesAsync(filePath.Value, request.Data.ToByteArray());
            // }
            // catch (Exception ex)
            // {
            //     return new UploadResponse { Status = $"Failed: {ex.Message}" };
            // }

        }
        else
        {
            
        }
        
        return new UploadResponse { Status = "Success" };
    }

    public bool DirectoryExistsOrNotEmpty(string path)
    {
        return Directory.Exists(path) ;
    }
}

