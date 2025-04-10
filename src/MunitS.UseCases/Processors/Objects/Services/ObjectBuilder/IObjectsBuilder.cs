using MunitS.Domain.Object.ObjectByBucketId;
using MunitS.Domain.Object.ObjectByParentPrefix;
using MunitS.UseCases.Processors.Objects.Services.ObjectBuilder;
namespace MunitS.UseCases.Processors.Objects.Services;

public interface IObjectsBuilder
{
    ObjectsBuilder ToInsert(ObjectByParentPrefix objectByParentPrefix);
    ObjectsBuilder ToInsert(ObjectByBucketId objectByBucketId);
    ObjectsBuilder ToDelete(ObjectsBuilder.DeleteObjectByParentPrefix payload);
    ObjectsBuilder ToDelete(ObjectsBuilder.DeleteObjectByBucketId payload);
    Task Build();
}
