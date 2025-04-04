using MunitS.Domain.Object.ObjectByFileKey;
using MunitS.Domain.Object.ObjectByParentPrefix;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByFileKeyRepository;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByParentPrefixRepository;
namespace MunitS.UseCases.Processors.Objects.Services;

public class ObjectsBuilder(IObjectByFileKeyRepository objectByFileKeyRepository, IObjectByParentPrefixRepository objectByParentPrefixRepository) : IObjectsBuilder
{
    private readonly List<ObjectByFileKey> _objectByFileKeys = [];
    private readonly List<ObjectByParentPrefix> _objectByParentPrefixes = [];

    public ObjectsBuilder Add(ObjectByFileKey objectByFileKey)
    {
        _objectByFileKeys.Add(objectByFileKey);
        return this;
    }

    public ObjectsBuilder Add(ObjectByParentPrefix objectByParentPrefix)
    {
        _objectByParentPrefixes.Add(objectByParentPrefix);
        return this;
    }

    public async Task Build()
    {
        var insertTasks = _objectByFileKeys.Select(objectByFileKeyRepository.Create).ToList()
            .Concat(_objectByParentPrefixes.Select(objectByParentPrefixRepository.Create));

        await Task.WhenAll(insertTasks);
    }
}
