using Cassandra.Mapping;
namespace MunitS.Domain.Object.ObjectByUploadId;

public class ObjectsByUploadIdMapping : Mappings
{
    private const string TableName = "objects_by_upload_id";

    public ObjectsByUploadIdMapping()
    {
        For<ObjectByUploadId>()
            .TableName(TableName)
            .PartitionKey(c => c.BucketId)
            .Column(c => c.Id, cm => cm.WithName("id"))
            .Column(c => c.BucketId, cm => cm.WithName("bucket_id"))
            .Column(c => c.DivisionId, cm => cm.WithName("division_id"))
            .Column(c => c.UploadId, cm => cm.WithName("upload_id"))
            .Column(c => c.InitiatedAt, cm => cm.WithName("initiated_at"))
            .Column(c => c.FileKey, cm => cm.WithName("file_key"))
            .Column(c => c.FileName, cm => cm.WithName("file_name"))
            .Column(c => c.UploadedAt, cm => cm.WithName("uploaded_at"))
            .Column(c => c.Extension, cm => cm.WithName("extension"))
            .Column(c => c.UploadStatus, cm => cm.WithName("upload_status"))
            .Column(c => c.DivisionSizeType, cm => cm.WithName("division_size_type"))
            .Column(c => c.MimeType, cm => cm.WithName("mime_type"))
            .Column(c => c.SizeInBytes, cm => cm.WithName("size_in_bytes"))
            .Column(c => c.CustomMetadata, cm => cm.WithName("custom_metadata"))
            .Column(c => c.Tags, cm => cm.WithName("tags"));
    }
}
