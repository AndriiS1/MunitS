using MunitS.Domain.Object.ObjectByBucketId;
using MunitS.Domain.Object.ObjectByFileKey;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByBucketIdRepository;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByFileKeyRepository;
namespace MunitS.UseCases.Processors.Objects.Services.ObjectBuilder;

public class ObjectsBuilder(IObjectByBucketIdRepository objectByBucketIdRepository,
    IObjectByFileKeyRepository objectByFileKeyRepository) : IObjectsBuilder
{
    private readonly List<DeleteObjectByBucketId> _objectByBucketIdsToDelete = [];
    private readonly List<ObjectByBucketId> _objectByBucketIdsToInsert = [];

    private readonly List<DeleteObjectByFileKey> _objectByFileKeysToDelete = [];
    private readonly List<ObjectByFileKey> _objectByFileKeysToInsert = [];

    public ObjectsBuilder ToDelete(DeleteObjectByBucketId payload)
    {
        _objectByBucketIdsToDelete.Add(payload);
        return this;
    }

    public ObjectsBuilder ToDelete(DeleteObjectByFileKey payload)
    {
        _objectByFileKeysToDelete.Add(payload);
        return this;
    }

    public ObjectsBuilder ToInsert(ObjectByBucketId objectByBucketId)
    {
        _objectByBucketIdsToInsert.Add(objectByBucketId);
        return this;
    }

    public ObjectsBuilder ToInsert(ObjectByFileKey objectByFileKey)
    {
        _objectByFileKeysToInsert.Add(objectByFileKey);
        return this;
    }

    public async Task Build()
    {
        var insertTasks = _objectByBucketIdsToInsert.Select(objectByBucketIdRepository.Create).ToList()
            .Concat(_objectByFileKeysToInsert.Select(objectByFileKeyRepository.Create));

        var deleteTasks = _objectByBucketIdsToDelete.Select(o => objectByBucketIdRepository.Delete(o.BucketId, o.UploadId))
            .Concat(_objectByFileKeysToDelete.Select(o => objectByFileKeyRepository.Delete(o.BucketId, o.FileKey, o.UploadId)));

        await Task.WhenAll(insertTasks.Concat(deleteTasks));
    }

    public sealed record DeleteObjectByBucketId(Guid BucketId, Guid UploadId);

    public sealed record DeleteObjectByFileKey(Guid BucketId, string FileKey, Guid UploadId);

    public sealed record DeleteObjectByParentPrefix(Guid BucketId, string FileName, string ParentPrefix);
}
