using Cassandra.Data.Linq;
namespace MunitS.Infrastructure.Data.Repositories.Part.PartByUploadId;

public class PartByUploadIdRepository(CassandraConnector connector) : IPartByUploadIdRepository
{
    private readonly Table<Domain.Part.PartByUploadId.PartByUploadId> _objects = new(connector.GetSession());

    public async Task<Domain.Part.PartByUploadId.PartByUploadId?> Get(Guid bucketId, Guid uploadId, int number)
    {
        return await _objects.FirstOrDefault(o => o.BucketId == bucketId && o.UploadId == uploadId && o.Number == number).ExecuteAsync();
    }

    public async Task<List<Domain.Part.PartByUploadId.PartByUploadId>> GetAll(Guid bucketId, Guid uploadId)
    {
        return (await _objects.Where(o => o.BucketId == bucketId && o.UploadId == uploadId).ExecuteAsync()).ToList();
    }

    public async Task Delete(Guid bucketId, Guid uploadId)
    {
        await _objects.Where(o => o.BucketId == bucketId && o.UploadId == uploadId).Delete().ExecuteAsync();
    }

    public async Task Create(Domain.Part.PartByUploadId.PartByUploadId partByUploadId)
    {
        await _objects.Insert(partByUploadId).ExecuteAsync();
    }
}
