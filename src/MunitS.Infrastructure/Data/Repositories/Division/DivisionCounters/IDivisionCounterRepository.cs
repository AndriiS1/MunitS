using MunitS.Domain.Division.DivisionByBucketId;
using MunitS.Domain.Division.DivisionCounter;
namespace MunitS.Infrastructure.Data.Repositories.Division.DivisionCounters;

public interface IDivisionCounterRepository
{
    Task<List<DivisionCounter>> GetAll(Guid bucketId, DivisionType.SizeType type);
    Task IncrementObjectsCount(Guid bucketId, DivisionType.SizeType type, Guid id, long increment = 1);
}
