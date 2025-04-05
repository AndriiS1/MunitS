using Microsoft.Extensions.Options;
using MunitS.Domain.Directory;
using MunitS.Infrastructure.Options.Storage;
namespace MunitS.UseCases.Processors.Service.PathRetriever;

public class PathRetriever(IOptions<StorageOptions> options) : IPathRetriever
{
    public string GetAbsoluteBucketDirectory(BucketDirectory bucketDirectory)
    {
        return Path.Combine(options.Value.RootDirectory, bucketDirectory.Value);
    }

    public string GetAbsoluteDivisionDirectory(DivisionDirectory divisionDirectory)
    {
        return Path.Combine(options.Value.RootDirectory, divisionDirectory.Value);
    }

    public string GetAbsoluteObjectTempVersionDirectory(TempObjectVersionDirectory objectDirectory)
    {
        return Path.Combine(options.Value.RootDirectory, objectDirectory.Value);
    }
    
    public string GetAbsoluteObjectVersionDirectory(string objectPath, Guid versionId)
    {
        return Path.Combine(options.Value.RootDirectory, objectPath, versionId.ToString());
    }
    
    public string GetAbsoluteObjectDirectory(string objectPath)
    {
        return Path.Combine(options.Value.RootDirectory, objectPath);
    }
}
