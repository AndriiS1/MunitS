namespace MunitS.Domain.Chunk;

public class ObjectPath(FileVersionedDirectory fileVersionedDirectory, string fileKey)
{
    private const string DataDirectory = "Data";
    public string Value { get; } = $"{fileVersionedDirectory.Value}/{DataDirectory}";
}
