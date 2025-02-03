namespace MunitS.Domain.Versioning.Configs;

public class DisabledVersioning(Guid mainVersionId): VersioningConfig
{
    public Guid MainVersionId { get; } = mainVersionId;
}
