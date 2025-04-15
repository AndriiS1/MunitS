using MunitS.Domain.Object.ObjectByBucketId;
using MunitS.Domain.Object.ObjectByFileKey;
using MunitS.Domain.Object.ObjectByParentPrefix;
namespace MunitS.UseCases.Processors.Objects.Services.ObjectBuilder;

public interface IObjectsBuilder
{
    ObjectsBuilder ToInsert(ObjectByParentPrefix objectByParentPrefix);
    ObjectsBuilder ToInsert(ObjectByBucketId objectByBucketId);
    ObjectsBuilder ToInsert(ObjectByFileKey objectByFileKey);
    ObjectsBuilder ToDelete(ObjectsBuilder.DeleteObjectByParentPrefix payload);
    ObjectsBuilder ToDelete(ObjectsBuilder.DeleteObjectByBucketId payload);
    ObjectsBuilder ToDelete(ObjectsBuilder.DeleteObjectByFileKey payload);
    Task Build();
}
