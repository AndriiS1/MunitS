using Microsoft.Extensions.Options;
using MunitS.Domain.Directory;
using MunitS.Infrastructure.Options.Storage;
namespace MunitS.UseCases.Processors.Service.PathRetriever;

public class PathRetriever(IOptions<StorageOptions> options) : IPathRetriever
{
    public string GetAbsoluteDirectoryPath(DirectoryBase directory)
    {
        return Path.Combine(options.Value.RootDirectory, directory.Value);
    }
}
