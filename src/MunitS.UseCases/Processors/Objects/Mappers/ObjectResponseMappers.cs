using MunitS.Domain.Object.ObjectByFileKey;
using MunitS.Domain.Object.ObjectByUploadId;
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
            Type = @object.Type,
            MimeType = @object.MimeType ?? "-",
            CreatedAt = @object.CreatedAt.ToString()
        };
    }

    private static ObjectSuffixesResponse GetObjectSuffixesResponse(ObjectSuffixesPage page)
    {
        var response = new ObjectSuffixesResponse
        {
            HasNext = page.HasNext
        };

        response.ObjectSuffixes.AddRange(page.Data.Select(FormatObjectSuffixesResponse));

        if (page.NextCursor != null)
        {
            response.NextCursor = new ObjectSuffixesCursor
            {
                Suffix = page.NextCursor.Suffix,
                Type = page.NextCursor?.Type.ToString()
            };
        }

        return response;
    }

    public static GetObjectsSuffixesResponse FormatObjectSuffixes(ObjectSuffixesPage page)
    {
        var response = new GetObjectsSuffixesResponse
        {
            Suffixes = GetObjectSuffixesResponse(page)
        };

        return response;
    }

    private static ObjectVersionResponse FormatGetObjectResponse(ObjectByUploadId objectByUploadId)
    {
        var response = new ObjectVersionResponse
        {
            UploadId = objectByUploadId.UploadId.ToString(),
            UploadStatus = objectByUploadId.UploadStatus,
            SizeInBytes = objectByUploadId.SizeInBytes,
            InitiatedAt = objectByUploadId.InitiatedAt.ToString(),
            MimeType = objectByUploadId.MimeType
        };

        foreach (var metadata in objectByUploadId.CustomMetadata)
        {
            response.CustomMetadata.Add(metadata.Key, metadata.Value);
        }

        foreach (var tag in objectByUploadId.Tags)
        {
            response.CustomMetadata.Add(tag.Key, tag.Value);
        }

        return response;
    }

    public static GetObjectResponse FormatObjectResponse(ObjectByFileKey objectByFileKey, List<ObjectByUploadId> objectByUploadIds)
    {
        var response = new GetObjectResponse
        {
            Id = objectByFileKey.Id.ToString(),
            CreatedAt = objectByFileKey.CreatedAt.ToString(),
            FileKey = objectByFileKey.FileKey
        };

        response.Versions.Add(objectByUploadIds.Select(FormatGetObjectResponse));

        return response;
    }
}
