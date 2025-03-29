using MunitS.Domain.FolderPrefixes.FolderPrefixByParentPrefix;
namespace MunitS.Infrastructure.Data.Repositories.FolderPrefix.FolderPrefixByParentPrefixRepository;

public interface IFolderPrefixByParentPrefixRepository
{
    Task<List<FolderPrefixByParentPrefix>> GetAll(Guid bucketId, string parentPrefix);
    public Task Delete(Guid bucketId, string parentPrefix);
    public Task Create(FolderPrefixByParentPrefix folderPrefixByParentPrefix);
}
