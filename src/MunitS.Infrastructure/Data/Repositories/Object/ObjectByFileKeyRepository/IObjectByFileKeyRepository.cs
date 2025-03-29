using MunitS.Domain.Object.ObjectByFileKey;
namespace MunitS.Infrastructure.Data.Repositories.Object.ObjectByFileKeyRepository;

public interface IObjectByFileKeyRepository
{
    public Task<ObjectByFileKey?> Get(string fileKey, Guid bucketId);
    public Task<List<ObjectByFileKey>> GetAll(string fileKey, Guid bucketId);
    public Task Delete(string fileKey, Guid bucketId, Guid versionId);
    public Task Create(ObjectByFileKey objectByFileKey);
}
