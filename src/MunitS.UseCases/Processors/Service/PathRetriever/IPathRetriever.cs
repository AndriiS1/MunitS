using MunitS.Domain.Directory;
namespace MunitS.UseCases.Processors.Service.PathRetriever;

public interface IPathRetriever
{
    string GetAbsoluteDirectoryPath(DirectoryBase directory);
}
