using MunitS.Domain.FolderPrefixes.FolderPrefixByParentPrefix;
namespace MunitS.UseCases.Processors.Service.ForlderPrefixesRetriever;

public static class FolderPrefixesRetriever
{
    public static
        List<FolderPrefixByParentPrefix> GetFolderPrefixes(Guid bucketId, string fileKey, Guid objectId)
    {
        var split = fileKey.Split("/");
        var folders = split[new Range(0, split.Length - 1)];

        List<FolderPrefixByParentPrefix> result = [];

        var parentPrefix = "/";

        foreach (var folder in folders)
        {
            result.Add(FolderPrefixByParentPrefix.Create(bucketId, parentPrefix, folder));
            parentPrefix = Path.Combine(parentPrefix, folder + "/");
        }

        return result;
    }
}
