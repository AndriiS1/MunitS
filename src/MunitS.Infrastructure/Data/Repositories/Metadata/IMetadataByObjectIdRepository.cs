using MunitS.Domain.Metadata.MedataByObjectId;
namespace MunitS.Infrastructure.Data.Repositories.Metadata;

public interface IMetadataByObjectIdRepository
{
    Task<MetadataByObjectId?> Get(Guid bucketId, Guid uploadId);
    Task Create(MetadataByObjectId metadataByObjectId);
    Task Delete(Guid bucketId, Guid uploadId);
}
