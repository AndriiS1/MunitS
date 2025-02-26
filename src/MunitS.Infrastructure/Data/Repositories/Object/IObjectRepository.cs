namespace MunitS.Infrastructure.Data.Repositories.Object;

public interface IObjectRepository
{
    public Task<Domain.Object.Object?> Get(string fileKey, Guid bucketId);
    public Task<List<Domain.Object.Object>> GetAll(string fileKey, Guid bucketId);
    public Task Delete(string fileKey, Guid bucketId, Guid versionId);
    public Task Create(Domain.Object.Object @object);
}
