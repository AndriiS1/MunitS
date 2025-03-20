using Cassandra.Mapping;
namespace MunitS.Domain.Object.ObjectByFileKey;

public class ObjectsByFileKeyMapping : Mappings
{
    private const string TableName = "objects_by_file_key";
    
    public ObjectsByFileKeyMapping()
    {
        For<ObjectByFileKey>()
            .TableName(TableName)
            .PartitionKey(c => c.BucketId, c=> c.FileKey)
            .Column(c => c.Id, cm => cm.WithName("id"))
            .Column(c => c.BucketId, cm => cm.WithName("bucket_id"))
            .Column(c => c.VersionId, cm => cm.WithName("version_id"))
            .Column(c => c.FileKey, cm => cm.WithName("file_key"))
            .Column(c => c.FileName, cm => cm.WithName("file_name"))
            .Column(c => c.UploadedAt, cm => cm.WithName("uploaded_at"))
            .Column(c => c.UploadStatus, cm => cm.WithName("upload_status"))
            .Column(c => c.Path, cm => cm.WithName("path"));
    }
}
