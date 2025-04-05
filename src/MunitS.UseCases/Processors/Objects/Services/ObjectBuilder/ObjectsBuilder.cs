using MunitS.Domain.Object.ObjectByFileKey;
using MunitS.Domain.Object.ObjectByParentPrefix;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByFileKeyRepository;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByParentPrefixRepository;
namespace MunitS.UseCases.Processors.Objects.Services.ObjectBuilder;

public class ObjectsBuilder(IObjectByFileKeyRepository objectByFileKeyRepository, IObjectByParentPrefixRepository objectByParentPrefixRepository) : IObjectsBuilder
{
    private readonly List<DeleteObjectByFileKey> _objectByFileKeysToDelete = [];
    private readonly List<ObjectByFileKey> _objectByFileKeysToInsert = [];
    private readonly List<DeleteObjectByParentPrefix> _objectByParentPrefixesToDelete = [];
    private readonly List<ObjectByParentPrefix> _objectByParentPrefixesToInsert = [];

    public ObjectsBuilder ToDelete(DeleteObjectByFileKey payload)
    {
        _objectByFileKeysToDelete.Add(payload);
        return this;
    }

    public ObjectsBuilder ToDelete(DeleteObjectByParentPrefix payload)
    {
        _objectByParentPrefixesToDelete.Add(payload);
        return this;
    }

    public ObjectsBuilder ToInsert(ObjectByFileKey objectByFileKey)
    {
        _objectByFileKeysToInsert.Add(objectByFileKey);
        return this;
    }

    public ObjectsBuilder ToInsert(ObjectByParentPrefix objectByParentPrefix)
    {
        _objectByParentPrefixesToInsert.Add(objectByParentPrefix);
        return this;
    }

    public async Task Build()
    {
        var insertTasks = _objectByFileKeysToInsert.Select(objectByFileKeyRepository.Create).ToList()
            .Concat(_objectByParentPrefixesToInsert.Select(objectByParentPrefixRepository.Create));

        var deleteTasks = _objectByFileKeysToDelete.Select(o => objectByFileKeyRepository.Delete(o.BucketId, o.FileKey, o.VersionId))
            .Concat(_objectByParentPrefixesToDelete.Select(o => objectByParentPrefixRepository.Delete(o.BucketId, o.ParentPrefix)));

        await Task.WhenAll(insertTasks.Concat(deleteTasks));
    }

    public sealed record DeleteObjectByFileKey(Guid BucketId, string FileKey, Guid VersionId);

    public sealed record DeleteObjectByParentPrefix(Guid BucketId, string ParentPrefix);
}
