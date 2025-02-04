using MunitS.Domain.Versioning;
namespace MunitS.Infrastructure.Data.Repositories.ObjectVersioning;

public interface IObjectVersionRepository
{
    public Task Create(ObjectVersion version);
    public Task<List<ObjectVersion>> GetAll(Guid objectId);
    public Task DeleteOldest(Guid objectId);
}
