using MunitS.Domain.Division.DivisionByBucketId;
namespace MunitS.UseCases.Processors.Objects.Services.DivisionBuilder;

public interface IDivisionBuilder
{
    DivisionBuilder ToInsert(DivisionByBucketId metadataByObjectId);
    Task Build();
}
