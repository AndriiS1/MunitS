namespace MunitS.Domain.Versioning.Configs;

public class EnabledVersioning: VersioningConfig
{
    public Queue<Guid> Versions { get; set; } = new();
}
