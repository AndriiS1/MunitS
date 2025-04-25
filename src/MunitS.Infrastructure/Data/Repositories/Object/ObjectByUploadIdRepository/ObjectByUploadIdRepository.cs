using Cassandra.Data.Linq;
using MunitS.Domain.Object.ObjectByUploadId;
namespace MunitS.Infrastructure.Data.Repositories.Object.ObjectByUploadIdRepository;

public class ObjectByUploadIdRepository(CassandraConnector connector) : IObjectByUploadIdRepository
{
    private readonly Table<ObjectByUploadId> _objects = new(connector.GetSession());

    public async Task<List<ObjectByUploadId>> GetAll(Guid bucketId, Guid objectId)
    {
        return (await _objects.Where(o => o.BucketId == bucketId && o.Id == objectId).ExecuteAsync()).ToList();
    }

    public async Task<ObjectByUploadId?> GetByUploadId(Guid bucketId, Guid objectId, Guid uploadId)
    {
        return await _objects.FirstOrDefault(o => o.BucketId == bucketId && o.Id == objectId && o.UploadId == uploadId).ExecuteAsync();
    }

    public async Task UpdateUploadStatus(Guid bucketId, Guid objectId, Guid uploadId, UploadStatus status)
    {
        await _objects
            .Where(o => o.BucketId == bucketId && o.Id == objectId && o.UploadId == uploadId)
            .Select(u => new
            {
                UploadStatus = status.ToString()
            })
            .Update()
            .ExecuteAsync();
    }

    public async Task Delete(Guid bucketId, Guid objectId)
    {
        await _objects.Where(o => o.BucketId == bucketId).Delete().ExecuteAsync();
    }

    public async Task Delete(Guid bucketId, Guid objectId, Guid uploadId)
    {
        await _objects.Where(o => o.UploadId == uploadId && o.Id == objectId && o.BucketId == bucketId).Delete().ExecuteAsync();
    }

    public async Task Create(ObjectByUploadId objectByUploadId)
    {
        await _objects.Insert(objectByUploadId).ExecuteAsync();
    }

    public async Task Delete(Guid bucketId)
    {
        await _objects.Where(b => b.BucketId == bucketId).Delete().ExecuteAsync();
    }
}
