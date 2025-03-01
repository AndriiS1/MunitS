using MunitS.Domain.Division;
namespace MunitS.Infrastructure.Data.Repositories.Division;

public interface IDivisionRepository
{
    Task Create(Domain.Division.Division metadata);
    Task<Domain.Division.Division?> GetNotFull(string bucketName, DivisionType divisionType);
}
