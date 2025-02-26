using MunitS.Domain.Bucket;
using MunitS.Protos;
namespace MunitS.UseCases.Services.Buckets.Mappers;

public static class ResponseMappers
{
    public static GetBucketResponse FormatGetBucketResponse(Bucket bucket)
    {
        return new GetBucketResponse
        {
            Content = FormatBucketResponse(bucket)
        };
    }

    private static BucketResponse FormatBucketResponse(Bucket bucket)
    {
        return new BucketResponse
        {
            Id = bucket.Id.ToString(),
            Name = bucket.Name,
            ObjectsCount = bucket.ObjectsCount,
            Size = bucket.SizeInBytes,
            VersioningEnabled = bucket.VersioningEnabled,
            VersionsLimit = bucket.VersionsLimit,
        };
    }
    
    public static GetBucketsResponse FormatGetBucketsResponse(List<Bucket> buckets)
    {
        var response = new GetBucketsResponse();
        response.Content.AddRange(buckets.Select(FormatBucketResponse));
        
        return response;
    }
}
