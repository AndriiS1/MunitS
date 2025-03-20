using MunitS.Domain.Bucket.BucketById;
namespace MunitS.Domain.Division.DivisionByBucketId;

public class DivisionDirectory(BucketDirectory bucketDirectory, string divisionName, DivisionType.SizeType divisionSizeType)
{
    public string Value { get; } = $"{bucketDirectory.Value}/{divisionName}.{DivisionType.GetSizeTypePrefix(divisionSizeType)}";
}
