using System.Text.Json.Serialization;
namespace MunitS.Domain.Versioning.Configs;

[JsonDerivedType(typeof(EnabledVersioning), nameof(EnabledVersioning))]
[JsonDerivedType(typeof(DisabledVersioning), nameof(DisabledVersioning))]
public abstract class VersioningConfig
{
    public DateTime Created { get; init; }
}
