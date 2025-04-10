using Cassandra.Data.Linq;
using MunitS.Domain.Object.ObjectByBucketId;
namespace MunitS.Infrastructure.Data.Repositories.Object.ObjectByBucketIdRepository;

public class ObjectByBucketIdRepository(CassandraConnector connector) : IObjectByBucketIdRepository
{
    private readonly Table<ObjectByBucketId> _objects = new(connector.GetSession());

    public async Task<ObjectByBucketId?> GetByUploadId(Guid bucketId, Guid uploadId)
    {
        return await _objects.FirstOrDefault(o => o.BucketId == bucketId && o.UploadId == uploadId).ExecuteAsync();
    }

    public async Task UpdateUploadStatus(Guid bucketId, Guid uploadId, UploadStatus status)
    {
        await _objects
            .Where(o => o.BucketId == bucketId && o.UploadId == uploadId)
            .Select(u => new
            {
                UploadStatus = status.ToString()
            })
            .Update()
            .ExecuteAsync();
    }

    public async Task Delete(Guid bucketId, Guid uploadId)
    {
        await _objects.Where(o => o.UploadId == uploadId && o.BucketId == bucketId).Delete().ExecuteAsync();
    }

    public async Task Create(ObjectByBucketId objectByBucketId)
    {
        await _objects.Insert(objectByBucketId).ExecuteAsync();
    }

    public async Task<List<ObjectByBucketId>> GetAll(Guid bucketId, string fileKey)
    {
        return (await _objects.Where(o => o.BucketId == bucketId && o.FileKey == fileKey).ExecuteAsync()).ToList();
    }
}
