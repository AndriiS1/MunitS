using MunitS.Domain.Bucket.BucketById;
using MunitS.Domain.Bucket.BucketCounter;
using MunitS.Protos;
namespace MunitS.UseCases.Processors.Buckets.Mappers;

public static class ResponseMappers
{
    private static BucketCountersResponse FromBucketCounter(BucketCounter bucketCounter)
    {
        return new BucketCountersResponse
        {
            ObjectsCount = bucketCounter.ObjectsCount,
            Size = bucketCounter.SizeInBytes
        };
    }

    public static BucketResponse FormatBucketResponse(BucketById bucketById, BucketCounter? counter)
    {
        return new BucketResponse
        {
            Id = bucketById.Id.ToString(),
            Name = bucketById.Name,
            VersioningEnabled = bucketById.VersioningEnabled,
            VersionsLimit = bucketById.VersionsLimit,
            Counter = counter is null ? null : FromBucketCounter(counter)
        };
    }

    public static GetBucketsResponse FormatGetBucketsResponse(List<(BucketById bucket, BucketCounter? counter)> buckets)
    {
        var response = new GetBucketsResponse();
        response.Buckets.AddRange(buckets.Select(bucketData => FormatBucketResponse(bucketData.bucket, bucketData.counter)));

        return response;
    }
}
