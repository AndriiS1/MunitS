namespace MunitS.Domain.Chunk;

public class ObjectDirectory(Guid objectId)
{
    public string Value { get; } = $"{objectId}";
}
