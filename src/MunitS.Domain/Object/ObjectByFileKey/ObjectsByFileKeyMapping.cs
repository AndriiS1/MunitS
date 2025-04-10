using Cassandra.Mapping;
namespace MunitS.Domain.Object.ObjectByFileKey;

public class ObjectsByFileKeyMapping : Mappings
{
    private const string TableName = "objects_by_file_key";

    public ObjectsByFileKeyMapping()
    {
        For<ObjectByFileKey>()
            .TableName(TableName)
            .PartitionKey(c => c.BucketId)
            .Column(c => c.Id, cm => cm.WithName("id"))
            .Column(c => c.BucketId, cm => cm.WithName("bucket_id"))
            .Column(c => c.UploadStatus, cm => cm.WithName("upload_status"))
            .Column(c => c.UploadId, cm => cm.WithName("upload_id"))
            .Column(c => c.FileKey, cm => cm.WithName("file_key"));
    }
}
