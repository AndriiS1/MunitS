using MediatR;
using MunitS.Domain.ObjectSuffix.ObjectSuffixByParentPrefix;
using MunitS.Infrastructure.Data.Repositories.ObjectSuffix.ObjectSuffixByParentPrefixRepository;
using MunitS.Infrastructure.Data.Repositories.ObjectSuffix.ObjectSuffixByParentPrefixRepository.Dtos;
using MunitS.Protos;
using MunitS.UseCases.Processors.Objects.Mappers;
namespace MunitS.UseCases.Processors.Objects.Queries.GetObjects;

public class UploadFileCommandHandler(IObjectSuffixByParentPrefixRepository objectSuffixByParentPrefixRepository)
    : IRequestHandler<GetObjectsQuery, GetObjectsSuffixesResponse>
{
    public async Task<GetObjectsSuffixesResponse> Handle(GetObjectsQuery query, CancellationToken cancellationToken)
    {
        var cursor = query.Request.Cursor is null ? new ObjectSuffixesPage.ObjectSuffixesPageCursor(PrefixType.Directory, string.Empty) :
            new ObjectSuffixesPage.ObjectSuffixesPageCursor(Enum.Parse<PrefixType>(query.Request.Cursor.Type), query.Request.Cursor.Suffix);

        var page = await objectSuffixByParentPrefixRepository
            .GetPage(Guid.Parse(query.Request.BucketId), query.Request.Prefix, query.Request.PageSize, cursor);

        return ObjectResponseMappers.FormatObjectSuffixes(page);
    }
}
