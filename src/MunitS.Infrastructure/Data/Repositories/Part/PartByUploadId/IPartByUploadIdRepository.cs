namespace MunitS.Infrastructure.Data.Repositories.Part.PartByUploadId;

public interface IPartByUploadIdRepository
{
    public Task<Domain.Part.PartByUploadId.PartByUploadId?> Get(Guid bucketId, Guid uploadId, int number);
    public Task<List<Domain.Part.PartByUploadId.PartByUploadId>> GetAll(Guid bucketId, Guid uploadId);
    public Task Delete(Guid bucketId, Guid uploadId);
    public Task Create(Domain.Part.PartByUploadId.PartByUploadId partByUploadId);
}
