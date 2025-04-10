using MunitS.Domain.Object.ObjectByBucketId;
namespace MunitS.Infrastructure.Data.Repositories.Object.ObjectByBucketIdRepository;

public interface IObjectByBucketIdRepository
{
    public Task<ObjectByBucketId?> Get(Guid bucketId, string fileKey);
    public Task<List<ObjectByBucketId>> GetAll(Guid bucketId, string fileKey);
    public Task<ObjectByBucketId?> GetByUploadId(Guid bucketId, Guid uploadId);
    public Task Delete(Guid bucketId, Guid uploadId);
    public Task Create(ObjectByBucketId objectByBucketId);
    Task UpdateUploadStatus(Guid bucketId, Guid uploadId, UploadStatus status);
}
