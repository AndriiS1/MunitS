using MunitS.Domain.Object.ObjectByParentPrefix;
namespace MunitS.Infrastructure.Data.Repositories.Object.ObjectByParentPrefixRepository;

public interface IObjectByParentPrefixRepository
{
    public Task<List<ObjectByParentPrefix>> GetAll(Guid bucketId, string parentPrefix);
    public Task Delete(Guid bucketId, string fileName, string parentPrefix);
    public Task Create(ObjectByParentPrefix objectByParentPrefix);
}
