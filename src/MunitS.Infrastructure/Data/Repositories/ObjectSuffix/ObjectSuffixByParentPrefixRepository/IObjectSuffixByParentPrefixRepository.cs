using MunitS.Domain.ObjectSuffix.ObjectSuffixByParentPrefix;
using MunitS.Infrastructure.Data.Repositories.ObjectSuffix.ObjectSuffixByParentPrefixRepository.Dtos;
namespace MunitS.Infrastructure.Data.Repositories.ObjectSuffix.ObjectSuffixByParentPrefixRepository;

public interface IObjectSuffixByParentPrefixRepository
{
    Task<ObjectSuffixesPage> GetPage(Guid bucketId, string parentPrefix, int pageSize, 
        ObjectSuffixesPage.ObjectSuffixesPageCursor? cursor = null);
    Task Delete(Guid bucketId, string parentPrefix, string prefix);
    public Task Create(ObjectSuffixByParentPrefix objectSuffixByParentPrefix);
}
