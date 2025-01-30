using MunitS.Domain.Chunk;
namespace MunitS.Domain.Versioning;

public class VersioningPath(FileDirectory fileDirectory)
{
    private const string ConfigFileName = "versioning.json";
    public string Value { get;} = $"{fileDirectory.Value}/{ConfigFileName}";
}
