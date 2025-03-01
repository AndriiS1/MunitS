using MunitS.Domain.Bucket;
namespace MunitS.Domain.Division;

public class DivisionDirectory(BucketDirectory bucketDirectory, string divisionName, DivisionType.SizeType divisionSizeType)
{
    public string Value { get; } = $"{bucketDirectory.Value}/{divisionName}.{DivisionType.GetSizeTypePrefix(divisionSizeType)}";
}
