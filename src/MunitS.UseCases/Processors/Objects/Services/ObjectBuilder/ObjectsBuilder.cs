using MunitS.Domain.Object.ObjectByFileKey;
using MunitS.Domain.Object.ObjectByUploadId;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByFileKeyRepository;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByUploadIdRepository;
namespace MunitS.UseCases.Processors.Objects.Services.ObjectBuilder;

public class ObjectsBuilder(IObjectByUploadIdRepository objectByUploadIdRepository,
    IObjectByFileKeyRepository objectByFileKeyRepository) : IObjectsBuilder
{
    private readonly List<DeleteObjectByBucketId> _objectByBucketIdsToDelete = [];
    private readonly List<ObjectByUploadId> _objectByBucketIdsToInsert = [];

    private readonly List<ObjectByFileKey> _objectByFileKeysToInsert = [];

    public ObjectsBuilder ToDelete(DeleteObjectByBucketId payload)
    {
        _objectByBucketIdsToDelete.Add(payload);
        return this;
    }

    public ObjectsBuilder ToInsert(ObjectByUploadId objectByUploadId)
    {
        _objectByBucketIdsToInsert.Add(objectByUploadId);
        return this;
    }

    public ObjectsBuilder ToInsert(ObjectByFileKey objectByFileKey)
    {
        _objectByFileKeysToInsert.Add(objectByFileKey);
        return this;
    }

    public async Task Build()
    {
        var insertTasks = _objectByBucketIdsToInsert.Select(objectByUploadIdRepository.Create).ToList()
            .Concat(_objectByFileKeysToInsert.Select(objectByFileKeyRepository.Create));

        var deleteTasks = _objectByBucketIdsToDelete
            .Select(o => objectByUploadIdRepository.Delete(o.BucketId, o.UploadId));

        await Task.WhenAll(insertTasks.Concat(deleteTasks));
    }

    public sealed record DeleteObjectByBucketId(Guid BucketId, Guid UploadId);
}
