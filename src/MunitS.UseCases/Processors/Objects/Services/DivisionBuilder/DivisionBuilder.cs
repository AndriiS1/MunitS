using MunitS.Domain.Division.DivisionByBucketId;
using MunitS.Infrastructure.Data.Repositories.Division.DivisionById;
namespace MunitS.UseCases.Processors.Objects.Services.DivisionBuilder;

public class DivisionBuilder(IDivisionByIdRepository divisionByIdRepository) : IDivisionBuilder
{
    private readonly List<DivisionByBucketId> _divisionsByBucketId = [];

    public DivisionBuilder ToInsert(DivisionByBucketId metadataByObjectId)
    {
        _divisionsByBucketId.Add(metadataByObjectId);
        return this;
    }

    public async Task Build()
    {
        var insertTasks = _divisionsByBucketId.Select(divisionByIdRepository.Create);

        await Task.WhenAll(insertTasks);
    }
}
