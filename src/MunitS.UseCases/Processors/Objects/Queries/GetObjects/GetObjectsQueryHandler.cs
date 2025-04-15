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
        PrefixType? cursorType = query.Request.Cursor?.Type is null ? null : Enum.Parse<PrefixType>(query.Request.Cursor.Type);
        var cursorSuffix = query.Request.Cursor?.Suffix; 
        
        var cursor = query.Request.Cursor is null ? null : new ObjectSuffixesPage.ObjectSuffixesPageCursor(cursorType, cursorSuffix);
        
        var page = await objectSuffixByParentPrefixRepository
            .GetPage(Guid.Parse(query.Request.BucketId), query.Request.Prefix, query.Request.PageSize, cursor);

        return ObjectResponseMappers.FormatObjectSuffixes(page);
    }
}
