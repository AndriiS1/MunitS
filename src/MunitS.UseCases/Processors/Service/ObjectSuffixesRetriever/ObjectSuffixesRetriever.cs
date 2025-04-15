using MunitS.Domain.ObjectSuffix.ObjectSuffixByParentPrefix;
namespace MunitS.UseCases.Processors.Service.ObjectSuffixesRetriever;

public static class ObjectSuffixesRetriever
{
    public static List<ObjectSuffixByParentPrefix> GetObjectSuffixes(Guid bucketId, string fileKey, Guid objectId, string mimeType)
    {
        var trimmedFileKey =  fileKey.Trim('/');
        var split = trimmedFileKey.Split("/");
        var folders = split[new Range(0, split.Length - 1)];
        var fileName = split[^1];

        List<ObjectSuffixByParentPrefix> folderPrefixes = [];

        var parentPrefix = "/";

        foreach (var folder in folders)
        {
            folderPrefixes.Add(ObjectSuffixByParentPrefix.Create(bucketId, Guid.NewGuid(), parentPrefix, folder, PrefixType.Directory));
            parentPrefix = Path.Combine(parentPrefix, folder + "/");
        }

        var objectParentPrefix = folderPrefixes.Count > 0 ? folderPrefixes.Last().ParentPrefix + folderPrefixes.Last().Suffix + "/" : "/";

        var objectPrefix = ObjectSuffixByParentPrefix.Create(bucketId, objectId, objectParentPrefix, fileName, PrefixType.Object, mimeType);
        folderPrefixes.Add(objectPrefix);

        return folderPrefixes;
    }
}
