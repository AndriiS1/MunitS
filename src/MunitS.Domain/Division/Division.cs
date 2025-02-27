using MunitS.Domain.Bucket;
namespace MunitS.Domain.Division;

public class Division
{
    public const string TableName = "division";
    public required string BucketName { get; init; }
    public required DivisionType.SizeType Type { get; init; }
    public required string Name { get; init; }
    public required Guid Id { get; init; }
    public required int ObjectsAmount { get; init; }
    public required int ObjectsAmountLimit { get; init; }
    public required string DivisionPath { get; init; }
    
    public static Division Create(string bucketName, DivisionType divisionType, BucketDirectory bucketDirectory)
    {
        var id = Guid.NewGuid();
        var name = $"division-{id}";
        return new Division
        {
            Id = id,
            Name = name,
            BucketName = bucketName,
            ObjectsAmountLimit = divisionType.ObjectsAmountLimit,
            Type = divisionType.Type,
            ObjectsAmount = 0,
            DivisionPath = new DivisionDirectory(bucketDirectory, name, divisionType.Type).Value
        };
    }
}
