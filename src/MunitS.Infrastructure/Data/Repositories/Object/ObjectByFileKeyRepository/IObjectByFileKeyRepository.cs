using MunitS.Domain.Object.ObjectByFileKey;
namespace MunitS.Infrastructure.Data.Repositories.Object.ObjectByFileKeyRepository;

public interface IObjectByFileKeyRepository
{
    public Task<ObjectByFileKey?> Get(Guid bucketId, string fileKey);
    public Task<List<ObjectByFileKey>> GetAll(Guid bucketId, string fileKey);
    public Task Delete(Guid bucketId, string fileKey, Guid versionId);
    public Task Create(ObjectByFileKey objectByFileKey);
    Task UpdateUploadStatus(Guid bucketId, string fileKey, UploadStatus status);
}
