using MunitS.Domain.Object.ObjectByFileKey;
using MunitS.Domain.Object.ObjectByParentPrefix;
namespace MunitS.UseCases.Processors.Objects.Services;

public interface IObjectsBuilder
{
    ObjectsBuilder Add(ObjectByParentPrefix objectByParentPrefix);
    ObjectsBuilder Add(ObjectByFileKey objectByFileKey);
    Task Build();
}
