using MunitS.Domain.Division.DivisionByBucketId;
namespace MunitS.Infrastructure.Data.Repositories.Division;

public interface IDivisionRepository
{
    Task Create(DivisionByBucketId metadata);
    Task<DivisionByBucketId?> GetNotFull(Guid bucketId, DivisionType divisionType);
}
