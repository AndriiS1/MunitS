using Cassandra.Mapping;
namespace MunitS.Domain.Part.PartByUploadId;

public class PartsByUploadIdMapping : Mappings
{
    private const string TableName = "parts_by_upload_id";

    public PartsByUploadIdMapping()
    {
        For<PartByUploadId>()
            .TableName(TableName)
            .PartitionKey(c => c.BucketId, c => c.UploadId)
            .Column(c => c.Id, cm => cm.WithName("id"))
            .Column(c => c.BucketId, cm => cm.WithName("bucket_id"))
            .Column(c => c.UploadId, cm => cm.WithName("upload_id"))
            .Column(c => c.Number, cm => cm.WithName("number"))
            .Column(c => c.ETag, cm => cm.WithName("etag"));
    }
}
