using MunitS.Domain.Object.ObjectByBucketId;
using MunitS.Domain.Object.ObjectByParentPrefix;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByBucketIdRepository;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByParentPrefixRepository;
namespace MunitS.UseCases.Processors.Objects.Services.ObjectBuilder;

public class ObjectsBuilder(IObjectByBucketIdRepository objectByBucketIdRepository, IObjectByParentPrefixRepository objectByParentPrefixRepository) : IObjectsBuilder
{
    private readonly List<DeleteObjectByBucketId> _objectByBucketIdsToDelete = [];
    private readonly List<ObjectByBucketId> _objectByBucketIdsToInsert = [];
    private readonly List<DeleteObjectByParentPrefix> _objectByParentPrefixesToDelete = [];
    private readonly List<ObjectByParentPrefix> _objectByParentPrefixesToInsert = [];

    public ObjectsBuilder ToDelete(DeleteObjectByBucketId payload)
    {
        _objectByBucketIdsToDelete.Add(payload);
        return this;
    }

    public ObjectsBuilder ToDelete(DeleteObjectByParentPrefix payload)
    {
        _objectByParentPrefixesToDelete.Add(payload);
        return this;
    }

    public ObjectsBuilder ToInsert(ObjectByBucketId objectByBucketId)
    {
        _objectByBucketIdsToInsert.Add(objectByBucketId);
        return this;
    }

    public ObjectsBuilder ToInsert(ObjectByParentPrefix objectByParentPrefix)
    {
        _objectByParentPrefixesToInsert.Add(objectByParentPrefix);
        return this;
    }

    public async Task Build()
    {
        var insertTasks = _objectByBucketIdsToInsert.Select(objectByBucketIdRepository.Create).ToList()
            .Concat(_objectByParentPrefixesToInsert.Select(objectByParentPrefixRepository.Create));

        var deleteTasks = _objectByBucketIdsToDelete.Select(o => objectByBucketIdRepository.Delete(o.BucketId, o.UploadId))
            .Concat(_objectByParentPrefixesToDelete.Select(o => objectByParentPrefixRepository.Delete(o.BucketId, o.FileName, o.ParentPrefix)));

        await Task.WhenAll(insertTasks.Concat(deleteTasks));
    }

    public sealed record DeleteObjectByBucketId(Guid BucketId, Guid UploadId);

    public sealed record DeleteObjectByParentPrefix(Guid BucketId, string FileName, string ParentPrefix);
}
