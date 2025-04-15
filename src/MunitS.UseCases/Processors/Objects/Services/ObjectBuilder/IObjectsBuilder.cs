using MunitS.Domain.Object.ObjectByBucketId;
using MunitS.Domain.Object.ObjectByFileKey;
namespace MunitS.UseCases.Processors.Objects.Services.ObjectBuilder;

public interface IObjectsBuilder
{
    ObjectsBuilder ToInsert(ObjectByBucketId objectByBucketId);
    ObjectsBuilder ToInsert(ObjectByFileKey objectByFileKey);
    ObjectsBuilder ToDelete(ObjectsBuilder.DeleteObjectByBucketId payload);
    ObjectsBuilder ToDelete(ObjectsBuilder.DeleteObjectByFileKey payload);
    Task Build();
}
