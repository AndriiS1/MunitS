using MunitS.Domain.Bucket.BucketById;
using MunitS.Protos;
namespace MunitS.UseCases.Processors.Buckets.Mappers;

public static class ResponseMappers
{
    public static GetBucketResponse FormatGetBucketResponse(BucketById bucketById)
    {
        return new GetBucketResponse
        {
            Content = FormatBucketResponse(bucketById)
        };
    }

    private static BucketResponse FormatBucketResponse(BucketById bucketById)
    {
        return new BucketResponse
        {
            Id = bucketById.Id.ToString(),
            Name = bucketById.Name,
            ObjectsCount = bucketById.ObjectsCount,
            Size = bucketById.SizeInBytes,
            VersioningEnabled = bucketById.VersioningEnabled,
            VersionsLimit = bucketById.VersionsLimit,
        };
    }
    
    public static GetBucketsResponse FormatGetBucketsResponse(List<BucketById> buckets)
    {
        var response = new GetBucketsResponse();
        response.Content.AddRange(buckets.Select(FormatBucketResponse));
        
        return response;
    }
}
