using MunitS.Domain.Metadata.MedataByObjectId;
using MunitS.Infrastructure.Data.Repositories.Metadata;
namespace MunitS.UseCases.Processors.Objects.Services.MetadataBuilder;

public class MetadataBuilder(IMetadataByObjectIdRepository metadataByObjectIdRepository) : IMetadataBuilder
{
    private readonly List<MetadataByObjectId> _metadataByObjectIds = [];
    private readonly List<DeleteMetadataByObjectId> _objectByParentPrefixesToDelete = [];

    public MetadataBuilder ToDelete(DeleteMetadataByObjectId payload)
    {
        _objectByParentPrefixesToDelete.Add(payload);
        return this;
    }

    public MetadataBuilder ToInsert(MetadataByObjectId metadataByObjectId)
    {
        _metadataByObjectIds.Add(metadataByObjectId);
        return this;
    }

    public async Task Build()
    {
        var insertTasks = _metadataByObjectIds.Select(metadataByObjectIdRepository.Create);

        var deleteTasks = _objectByParentPrefixesToDelete.Select(o => metadataByObjectIdRepository.Delete(o.BucketId, o.UploadId));

        await Task.WhenAll(insertTasks.Concat(deleteTasks));
    }

    public sealed record DeleteMetadataByObjectId(Guid BucketId, Guid UploadId);
}
