namespace MunitS.Infrastructure.Data.Repositories.Object;

public interface IObjectRepository
{
    public Task<Domain.Object.ObjectByFileKey.ObjectByFileKey?> Get(string fileKey, Guid bucketId);
    public Task<List<Domain.Object.ObjectByFileKey.ObjectByFileKey>> GetAll(string fileKey, Guid bucketId);
    public Task Delete(string fileKey, Guid bucketId, Guid versionId);
    public Task Create(Domain.Object.ObjectByFileKey.ObjectByFileKey objectByFileKey);
}
