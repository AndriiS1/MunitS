namespace MunitS.Domain.Versioning.Configs;

public class EnabledVersioning
{
    public Queue<Guid> Versions { get; set; } = new();
}
