namespace MunitS.Infrastructure.Data.Repositories.Metadata;

public interface IMetadataRepository
{
    void Create(Domain.Metadata.MedataByObjectId.MetadataByObjectId metadataByObjectId);
}
