using MunitS.Domain.Object.ObjectByBucketId;
namespace MunitS.Infrastructure.Data.Repositories.Object.ObjectByBucketIdRepository;

public interface IObjectByBucketIdRepository
{
    public Task<ObjectByBucketId?> GetByUploadId(Guid bucketId, Guid uploadId);
    public Task Delete(Guid bucketId, Guid uploadId);
    public Task Create(ObjectByBucketId objectByBucketId);
    Task UpdateUploadStatus(Guid bucketId, Guid uploadId, UploadStatus status);
}
