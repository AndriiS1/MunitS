using Cassandra.Data.Linq;
using MunitS.Domain.Object.ObjectByFileKey;
using MunitS.Domain.Object.ObjectByUploadId;
namespace MunitS.Infrastructure.Data.Repositories.Object.ObjectByFileKeyRepository;

public class ObjectByFileKeyRepository(CassandraConnector connector) : IObjectByFileKeyRepository
{
    private readonly Table<ObjectByFileKey> _objects = new(connector.GetSession());

    public async Task<List<ObjectByFileKey>> GetAll(Guid bucketId, string fileKey)
    {
        return (await _objects.Where(o => o.BucketId == bucketId && o.FileKey == fileKey).ExecuteAsync()).ToList();
    }

    public async Task Delete(Guid bucketId, string fileKey, Guid uploadId)
    {
        await _objects.Where(o => o.FileKey == fileKey && o.BucketId == bucketId && o.UploadId == uploadId).Delete().ExecuteAsync();
    }

    public async Task UpdateUploadStatus(Guid bucketId, string fileKey, Guid uploadId, UploadStatus status)
    {
        await _objects
            .Where(o => o.BucketId == bucketId && o.FileKey == fileKey && o.UploadId == uploadId)
            .Select(u => new
            {
                UploadStatus = status.ToString()
            })
            .Update()
            .ExecuteAsync();
    }

    public async Task Create(ObjectByFileKey objectByFileKey)
    {
        await _objects.Insert(objectByFileKey).ExecuteAsync();
    }
}
