using MunitS.Domain.Object.ObjectByFileKey;
namespace MunitS.Infrastructure.Data.Repositories.Object.ObjectByFileKeyRepository;

public interface IObjectByFileKeyRepository
{
    public Task<ObjectByFileKey?> Get(Guid bucketId, string fileKey);
    public Task Delete(Guid bucketId, string fileKey);
    public Task Create(ObjectByFileKey objectByFileKey);
    Task Delete(Guid bucketId);
}
