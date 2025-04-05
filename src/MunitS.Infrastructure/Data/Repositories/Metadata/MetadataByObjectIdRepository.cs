using Cassandra.Data.Linq;
using MunitS.Domain.Metadata.MedataByObjectId;
namespace MunitS.Infrastructure.Data.Repositories.Metadata;

public class MetadataByObjectIdRepository(CassandraConnector connector) : IMetadataByObjectIdRepository
{

    private readonly Table<MetadataByObjectId> _metadata = new(connector.GetSession());

    public async Task Create(MetadataByObjectId metadataByObjectId)
    {
        await _metadata.Insert(metadataByObjectId).ExecuteAsync();
    }

    public async Task Delete(Guid bucketId, Guid objectId, Guid versionId)
    {
        await _metadata.Where(o => o.ObjectId == objectId &&
                                   o.BucketId == bucketId && o.VersionId == versionId).Delete().ExecuteAsync();
    }
}
