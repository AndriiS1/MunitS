using System.Text.Json.Serialization;
namespace MunitS.Domain.Versioning.Configs;

[JsonDerivedType(typeof(EnabledVersioning), typeDiscriminator: nameof(EnabledVersioning))]
[JsonDerivedType(typeof(DisabledVersioning), typeDiscriminator: nameof(DisabledVersioning))]
public abstract class VersioningConfig
{
    public DateTime Created { get; init; }
}
