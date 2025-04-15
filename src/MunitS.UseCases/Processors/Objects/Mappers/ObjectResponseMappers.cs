using MunitS.Domain.ObjectSuffix.ObjectSuffixByParentPrefix;
using MunitS.Infrastructure.Data.Repositories.ObjectSuffix.ObjectSuffixByParentPrefixRepository.Dtos;
using MunitS.Protos;
namespace MunitS.UseCases.Processors.Objects.Mappers;

public static class ObjectResponseMappers
{
    private static ObjectSuffixResponse FormatObjectSuffixesResponse(ObjectSuffixByParentPrefix @object)
    {
        return new ObjectSuffixResponse
        {
            Id = @object.Id.ToString(),
           Suffix = @object.Suffix,
           Type = @object.Type.ToString(),
           MimeType = @object.MimeType,
           CreatedAt = @object.CreatedAt.ToString(),
        };
    }

    private static ObjectSuffixesResponse GetObjectSuffixesResponse(ObjectSuffixesPage page)
    {
        var response = new ObjectSuffixesResponse
        {
            HasNext = page.HasNext,
        };
        
        response.ObjectSuffixes.AddRange(page.Data.Select(FormatObjectSuffixesResponse));

        if (page.NextCursor != null)
        {
            response.NextCursor = new ObjectSuffixesCursor
            {
                Suffix = page.NextCursor.Suffix,
                Type = page.NextCursor.Type.ToString(),
            };
        }
        
        return response;
    }
    
    public static GetObjectsSuffixesResponse FormatObjectSuffixes(ObjectSuffixesPage page)
    {
        var response = new GetObjectsSuffixesResponse
        {
            Status = "Success",
            Content = GetObjectSuffixesResponse(page)
        };
        
        return response;
    }
}
