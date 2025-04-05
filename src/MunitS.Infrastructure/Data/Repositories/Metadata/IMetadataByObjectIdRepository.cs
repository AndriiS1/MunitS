using MunitS.Domain.Metadata.MedataByObjectId;
namespace MunitS.Infrastructure.Data.Repositories.Metadata;

public interface IMetadataByObjectIdRepository
{
    Task Create(MetadataByObjectId metadataByObjectId);

    Task Delete(Guid bucketId, Guid objectId, Guid versionId);
}
