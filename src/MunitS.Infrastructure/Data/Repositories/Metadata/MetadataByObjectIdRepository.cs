using Cassandra.Data.Linq;
using MunitS.Domain.Metadata.MedataByObjectId;
namespace MunitS.Infrastructure.Data.Repositories.Metadata;

public class MetadataByObjectIdRepository(CassandraConnector connector) : IMetadataByObjectIdRepository
{
    private readonly Table<MetadataByObjectId> _metadata = new(connector.GetSession());

    public async Task<MetadataByObjectId?> Get(Guid bucketId, Guid uploadId)
    {
        return await _metadata.FirstOrDefault(b => b.BucketId == bucketId && b.UploadId == uploadId).ExecuteAsync();
    }

    public async Task Create(MetadataByObjectId metadataByObjectId)
    {
        await _metadata.Insert(metadataByObjectId).ExecuteAsync();
    }

    public async Task Delete(Guid bucketId, Guid versionId)
    {
        await _metadata.Where(o =>
            o.BucketId == bucketId && o.UploadId == versionId).Delete().ExecuteAsync();
    }
}
