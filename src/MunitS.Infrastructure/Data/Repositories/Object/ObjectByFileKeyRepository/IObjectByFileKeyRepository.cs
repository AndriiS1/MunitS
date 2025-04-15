using MunitS.Domain.Object.ObjectByBucketId;
using MunitS.Domain.Object.ObjectByFileKey;
namespace MunitS.Infrastructure.Data.Repositories.Object.ObjectByFileKeyRepository;

public interface IObjectByFileKeyRepository
{
    public Task<List<ObjectByFileKey>> GetAll(Guid bucketId, string fileKey);
    public Task Delete(Guid bucketId, string fileKey, Guid uploadId);
    public Task Create(ObjectByFileKey objectByFileKey);
    Task UpdateUploadStatus(Guid bucketId, string fileKey, Guid uploadId, UploadStatus status);
}
