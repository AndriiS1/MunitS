using MunitS.Domain.Object.ObjectByFileKey;
using MunitS.Domain.Object.ObjectByUploadId;
namespace MunitS.UseCases.Processors.Objects.Services.ObjectBuilder;

public interface IObjectsBuilder
{
    ObjectsBuilder ToInsert(ObjectByUploadId objectByUploadId);
    ObjectsBuilder ToInsert(ObjectByFileKey objectByFileKey);
    ObjectsBuilder ToDelete(ObjectsBuilder.DeleteObjectByBucketId payload);
    ObjectsBuilder ToDelete(ObjectsBuilder.DeleteObjectByFileKey payload);
    Task Build();
}
