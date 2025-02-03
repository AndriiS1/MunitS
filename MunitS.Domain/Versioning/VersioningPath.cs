using MunitS.Domain.Chunk;
namespace MunitS.Domain.Versioning;

public class VersioningPath(ObjectDirectory objectDirectory)
{
    private const string ConfigFileName = "versioning.json";
    public string Value { get;} = $"{objectDirectory.Value}/{ConfigFileName}";
}
