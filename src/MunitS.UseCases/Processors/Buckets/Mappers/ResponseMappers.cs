using MunitS.Domain.Bucket.BucketById;
using MunitS.Domain.Bucket.BucketCounter;
using MunitS.Protos;
namespace MunitS.UseCases.Processors.Buckets.Mappers;

public static class ResponseMappers
{
    public static GetBucketResponse FormatGetBucketResponse(BucketById bucketById, BucketCounter bucketCounter)
    {
        return new GetBucketResponse
        {
            Content = new ExtendedBucketResponse
            {
                Id = bucketById.Id.ToString(),
                Name = bucketById.Name,
                VersioningEnabled = bucketById.VersioningEnabled,
                VersionsLimit = bucketById.VersionsLimit,
                ObjectsCount = bucketCounter.ObjectsCount,
                Size = bucketCounter.SizeInBytes
            }
        };
    }

    private static BucketResponse FormatBucketResponse(BucketById bucketById)
    {
        return new BucketResponse
        {
            Id = bucketById.Id.ToString(),
            Name = bucketById.Name,
            VersioningEnabled = bucketById.VersioningEnabled,
            VersionsLimit = bucketById.VersionsLimit
        };
    }

    public static GetBucketsResponse FormatGetBucketsResponse(List<BucketById> buckets)
    {
        var response = new GetBucketsResponse();
        response.Content.AddRange(buckets.Select(FormatBucketResponse));

        return response;
    }
}
