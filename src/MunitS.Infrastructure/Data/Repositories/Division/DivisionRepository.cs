using Cassandra.Data.Linq;
using MunitS.Domain.Division;
namespace MunitS.Infrastructure.Data.Repositories.Division;

public class DivisionRepository(CassandraConnector connector) : IDivisionRepository
{
    private readonly Table<Domain.Division.Division> _divisions = new (connector.GetSession());
    
    public async Task Create(Domain.Division.Division metadata)
    {
        await _divisions.Insert(metadata).ExecuteAsync();
    }
    
    public async Task<Domain.Division.Division?> GetNotFull(string bucketName, DivisionType divisionType)
    {
        return await _divisions.FirstOrDefault(d => d.BucketName == bucketName 
                                              && d.Type == divisionType.Type && d.ObjectsAmountLimit < divisionType.ObjectsAmountLimit).ExecuteAsync();
    }
}
