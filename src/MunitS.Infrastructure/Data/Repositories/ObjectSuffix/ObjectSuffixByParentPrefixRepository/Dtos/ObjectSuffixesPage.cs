using MunitS.Domain.ObjectSuffix.ObjectSuffixByParentPrefix;
namespace MunitS.Infrastructure.Data.Repositories.ObjectSuffix.ObjectSuffixByParentPrefixRepository.Dtos;

public class ObjectSuffixesPage
{
    public List<ObjectSuffixByParentPrefix> Data { get; init; } = [];
    public ObjectSuffixesPageCursor? NextCursor { get; init; }
    public bool HasNext { get; init; }

    public record ObjectSuffixesPageCursor(PrefixType Type, string Suffix);
}
