using Cassandra.Mapping;
namespace MunitS.Domain.Object.ObjectByBucketId;

public class ObjectsByBucketIdMapping : Mappings
{
    private const string TableName = "objects_by_bucket_id";

    public ObjectsByBucketIdMapping()
    {
        For<ObjectByBucketId>()
            .TableName(TableName)
            .PartitionKey(c => c.BucketId)
            .Column(c => c.Id, cm => cm.WithName("id"))
            .Column(c => c.BucketId, cm => cm.WithName("bucket_id"))
            .Column(c => c.DivisionId, cm => cm.WithName("division_id"))
            .Column(c => c.VersionId, cm => cm.WithName("version_id"))
            .Column(c => c.UploadId, cm => cm.WithName("upload_id"))
            .Column(c => c.InitiatedAt, cm => cm.WithName("initiated_at"))
            .Column(c => c.FileKey, cm => cm.WithName("file_key"))
            .Column(c => c.FileName, cm => cm.WithName("file_name"))
            .Column(c => c.UploadedAt, cm => cm.WithName("uploaded_at"))
            .Column(c => c.Extension, cm => cm.WithName("extension"))
            .Column(c => c.UploadStatus, cm => cm.WithName("upload_status"))
            .Column(c => c.DivisionSizeType, cm => cm.WithName("division_size_type"));
    }
}
