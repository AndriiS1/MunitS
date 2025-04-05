using MunitS.Domain.Metadata.MedataByObjectId;
namespace MunitS.UseCases.Processors.Objects.Services.MetadataBuilder;

public interface IMetadataBuilder
{
    MetadataBuilder ToInsert(MetadataByObjectId metadataByObjectId);
    MetadataBuilder ToDelete(MetadataBuilder.DeleteMetadataByObjectId payload);
    Task Build();
}
