namespace MunitS.Domain.Bucket.BucketByName;

public class BucketByName
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    
    public static BucketByName Create(Guid id, string name)
    {
        return new BucketByName
        {
            Id = id,
            Name = name
        };
    }
}