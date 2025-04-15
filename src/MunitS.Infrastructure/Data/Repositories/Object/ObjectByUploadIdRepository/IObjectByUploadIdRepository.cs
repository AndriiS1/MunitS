using MunitS.Domain.Object.ObjectByUploadId;
namespace MunitS.Infrastructure.Data.Repositories.Object.ObjectByUploadIdRepository;

public interface IObjectByUploadIdRepository
{
    Task<List<ObjectByUploadId>> GetAll(Guid bucketId, Guid objectId);
    Task<ObjectByUploadId?> GetByUploadId(Guid bucketId, Guid objectId, Guid uploadId);
    Task Delete(Guid bucketId, Guid uploadId);
    Task Delete(Guid bucketId, Guid objectId, Guid uploadId);
    Task Create(ObjectByUploadId objectByUploadId);
    Task UpdateUploadStatus(Guid bucketId, Guid objectId, Guid uploadId, UploadStatus status);
}
