using MunitS.Domain.Chunk;
using MunitS.Domain.Versioning.Configs;
namespace MunitS.Domain.Versioning;

public class Versioning(FileDirectory fileDirectory)
{
    private VersioningPath Path { get; } = new(fileDirectory);
    
    private VersioningConfig Config { get; }
}
