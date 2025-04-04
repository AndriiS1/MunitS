using MunitS.Domain.Object.ObjectByFileKey;
using MunitS.Domain.Object.ObjectByParentPrefix;
namespace MunitS.UseCases.Processors.Objects.Services;

public interface IObjectsBuilder
{
    ObjectsBuilder ToInsert(ObjectByParentPrefix objectByParentPrefix);
    ObjectsBuilder ToInsert(ObjectByFileKey objectByFileKey);
    ObjectsBuilder ToDelete(ObjectsBuilder.DeleteObjectByParentPrefix payload);
    ObjectsBuilder ToDelete(ObjectsBuilder.DeleteObjectByFileKey payload);
    Task Build();
}
