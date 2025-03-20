using Cassandra.Data.Linq;

namespace MunitS.Infrastructure.Data.Repositories.Metadata;

public class MetadataRepository(CassandraConnector connector) : IMetadataRepository
{
    
    private readonly Table<Domain.Metadata.MedataByObjectId.MetadataByObjectId> _metadata = new (connector.GetSession());
    
    public void Create(Domain.Metadata.MedataByObjectId.MetadataByObjectId metadataByObjectId)
    {
        _metadata.Insert(metadataByObjectId);
    }
}
