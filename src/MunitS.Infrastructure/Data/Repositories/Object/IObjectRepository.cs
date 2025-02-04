namespace MunitS.Infrastructure.Data.Repositories.Object;

public interface IObjectRepository
{
    public Task<Domain.Object.Object?> Get(Guid bucketId, string fileKey);
    public Task Create(Domain.Object.Object @object);
}
