using MunitS.Domain.Object.ObjectByFileKey;
using MunitS.Domain.Object.ObjectByUploadId;
namespace MunitS.Infrastructure.Data.Repositories.Object.ObjectByFileKeyRepository;

public interface IObjectByFileKeyRepository
{
    public Task<List<ObjectByFileKey>> GetAll(Guid bucketId, string fileKey);
    public Task<ObjectByFileKey?> Any(Guid bucketId, string fileKey);
    public Task Delete(Guid bucketId, string fileKey, Guid uploadId);
    public Task Create(ObjectByFileKey objectByFileKey);
    Task UpdateUploadStatus(Guid bucketId, string fileKey, Guid uploadId, UploadStatus status);
}
