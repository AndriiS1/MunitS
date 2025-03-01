using MunitS.Domain.Bucket;
namespace MunitS.Domain.Division;

public class Division
{
    public const string TableName = "divisions";
    public required string BucketName { get; init; }
    public required string Type { get; init; }
    public required string Name { get; init; }
    public required Guid Id { get; init; }
    public required long ObjectsCount { get; init; }
    public required long ObjectsLimit { get; init; }
    public required string Path { get; init; }

    public DivisionType.SizeType GetSizeType()
    {
        return Enum.Parse<DivisionType.SizeType>(Type);
    }
    
    public static Division Create(string bucketName, DivisionType divisionType, BucketDirectory bucketDirectory)
    {
        var id = Guid.NewGuid();
        var name = $"division-{id}";
        return new Division
        {
            Id = id,
            Name = name,
            BucketName = bucketName,
            ObjectsLimit = divisionType.ObjectsCountLimit,
            Type = divisionType.Type.ToString(),
            ObjectsCount = 0,
            Path = new DivisionDirectory(bucketDirectory, name, divisionType.Type).Value
        };
    }
}
