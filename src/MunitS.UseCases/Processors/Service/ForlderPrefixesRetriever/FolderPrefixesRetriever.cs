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

        var prefix = "/";

        foreach (var folder in folders)
        {
            result.Add(FolderPrefixByParentPrefix.Create(bucketId, folder, prefix));
            prefix = Path.Combine(prefix, folder + "/");
        }

        return result;
    }
}
