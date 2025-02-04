using Cassandra;
namespace MunitS.Domain.Versioning;

public class ObjectVersion(Guid objectId, DateTime uploadTime)
{
    public Guid Id { get; private init; } = Guid.NewGuid();
    public Guid ObjectId { get; } = objectId;
    private DateTime Uploaded { get; } = uploadTime;
    
    public static ObjectVersion FromDataInstance(Row row)
    {
        return new ObjectVersion(row.GetValue<Guid>(nameof(ObjectId)), row.GetValue<DateTime>(nameof(Uploaded)))
        {
            Id = row.GetValue<Guid>(nameof(Id)),
        };
    }
    
    public string ToCreateInstance()
    {
        return $"({Id}, {ObjectId}, {Uploaded})";
    }
}
