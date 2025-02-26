using Cassandra.Data.Linq;

namespace MunitS.Infrastructure.Data.Repositories.Metadata;

public class MetadataRepository(CassandraConnector connector) : IMetadataRepository
{
    
    private readonly Table<Domain.Metadata.Metadata> _metadata = new (connector.GetSession());
    
    public void Create(Domain.Metadata.Metadata metadata)
    {
        _metadata.Insert(metadata);
    }
}
