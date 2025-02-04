using Cassandra;
namespace MunitS.Domain.Bucket;

public class Bucket(string name)
{
    private const int MaxVersions = 10;
    public Guid Id { get; private init; } = Guid.NewGuid();
    public string Name { get; private init;} = name;
    public bool VersioningEnabled { get; private init; } = true;
    public int VersionsLimit { get; private init; } = MaxVersions;

    public static Bucket FromDataInstance(Row row)
    {
        return new Bucket(row.GetValue<string>(nameof(Name)))
        {
            Id = row.GetValue<Guid>(nameof(Id)),
            VersioningEnabled = row.GetValue<bool>(nameof(VersioningEnabled)),
            VersionsLimit = row.GetValue<int>(nameof(VersionsLimit))
        };
    }
    
    public string ToCreateInstance()
    {
        return $"({Id}, {Name}, {VersioningEnabled}, {VersionsLimit})";
    }
}
