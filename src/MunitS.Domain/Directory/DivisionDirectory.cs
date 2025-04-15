using MunitS.Domain.Division.DivisionByBucketId;
namespace MunitS.Domain.Directory;

public class DivisionDirectory(string bucketName, Guid divisionId, DivisionType.SizeType divisionSizeType) : DirectoryBase
{
    public override string Value { get; } = Path.Combine(bucketName, $"{divisionId.ToString()}.{DivisionType.GetSizeTypePrefix(divisionSizeType)}");
}
