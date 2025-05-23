using MunitS.Domain.ObjectSuffix.ObjectSuffixByParentPrefix;
using MunitS.Infrastructure.Data.Repositories.ObjectSuffix.ObjectSuffixByParentPrefixRepository.Dtos;
namespace MunitS.Infrastructure.Data.Repositories.ObjectSuffix.ObjectSuffixByParentPrefixRepository;

public interface IObjectSuffixByParentPrefixRepository
{
    Task<ObjectSuffixesPage> GetPage(Guid bucketId, string parentPrefix, int pageSize,
        ObjectSuffixesPage.ObjectSuffixesPageCursor cursor);

    Task Delete(Guid bucketId, string parentPrefix, PrefixType type, string suffix);
    Task Delete(Guid bucketId, string parentPrefix);
    public Task Create(ObjectSuffixByParentPrefix objectSuffixByParentPrefix);
    Task<List<ObjectSuffixByParentPrefix>> FetchTwoAsync(Guid bucketId, string parentPrefix);
    Task Delete(Guid bucketId);
}
