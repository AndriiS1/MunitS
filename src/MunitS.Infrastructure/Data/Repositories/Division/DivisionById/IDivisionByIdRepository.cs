using MunitS.Domain.Division.DivisionByBucketId;
namespace MunitS.Infrastructure.Data.Repositories.Division.DivisionById;

public interface IDivisionByIdRepository
{
    Task Create(DivisionByBucketId metadata);
    Task Delete(Guid bucketId);
    Task<List<DivisionByBucketId>> GetAll(Guid bucketId, DivisionType.SizeType type);
}
