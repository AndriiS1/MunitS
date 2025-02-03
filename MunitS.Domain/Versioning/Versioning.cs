using MunitS.Domain.Chunk;
using MunitS.Domain.Versioning.Configs;
namespace MunitS.Domain.Versioning;

public class Versioning(ObjectDirectory objectDirectory, VersioningConfig config, string fileKey)
{
    public string FileKey { get; } = fileKey;
    public VersioningPath Path { get; } = new(objectDirectory);

    private VersioningConfig Config { get; } = config;
}
