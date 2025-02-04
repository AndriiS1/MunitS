namespace MunitS.Infrastructure.Data.Repositories.Metadata;

public interface IMetadataRepository
{
    Task Create(Domain.Metadata.Metadata metadata);
    Task<Domain.Metadata.Metadata?> Get(string fileKey);
}
