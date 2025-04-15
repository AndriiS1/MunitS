using MunitS.Domain.Prefix.PrefixByParentPrefix;
namespace MunitS.Infrastructure.Data.Repositories.FolderPrefix.FolderPrefixByParentPrefixRepository;

public interface IFolderPrefixByParentPrefixRepository
{
    Task<List<FolderPrefixByParentPrefix>> GetAll(Guid bucketId, string parentPrefix);
    Task Delete(Guid bucketId, string parentPrefix, string prefix);
    public Task Create(FolderPrefixByParentPrefix folderPrefixByParentPrefix);
}
