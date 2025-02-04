using Cassandra;
namespace MunitS.Domain.Object;

public class Object(Guid bucketId)
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid BucketId { get; init; } = bucketId;
    public string FileKey { get; init; } = string.Empty;
    
    public static Object FromDataInstance(Row row)
    {
        return new Object(row.GetValue<Guid>(nameof(BucketId)))
        {
            Id = row.GetValue<Guid>(nameof(Id)),
            FileKey = row.GetValue<string>(nameof(FileKey))
        };
    }

    public string ToCreateInstance()
    {
        return $"({Id}, {BucketId}, {FileKey})";
    }
}
