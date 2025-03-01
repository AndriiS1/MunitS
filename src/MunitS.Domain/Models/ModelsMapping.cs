using Cassandra.Mapping;
namespace MunitS.Domain.Models;

public class ModelsMapping: Mappings
{
    public ModelsMapping()
    {
        For<Bucket.Bucket>()
            .TableName(Bucket.Bucket.TableName)
            .PartitionKey( c => c.Name)
            .Column(c => c.Id, cm => cm.WithName("id"))
            .Column(c => c.Name, cm => cm.WithName("name"))
            .Column(c => c.VersioningEnabled, cm => cm.WithName("versioning_enabled"))
            .Column(c => c.VersionsLimit, cm => cm.WithName("versions_limit"))
            .Column(c => c.ObjectsCount, cm => cm.WithName("objects_count"))
            .Column(c => c.SizeInBytes, cm => cm.WithName("size_in_bytes"));
        
        For<Division.Division>()
            .PartitionKey(c => c.BucketName)
            .PartitionKey(c => c.Type)
            .Column(c => c.BucketName, cm => cm.WithName("bucket_name"))
            .Column(c => c.Type, cm => cm.WithName("type"))
            .Column(c => c.Name, cm => cm.WithName("name"))
            .Column(c => c.Id, cm => cm.WithName("id"))
            .Column(c => c.ObjectsCount, cm => cm.WithName("objects_count"))
            .Column(c => c.ObjectsLimit, cm => cm.WithName("objects_limit"))
            .Column(c => c.Path, cm => cm.WithName("path"))
            .TableName(Division.Division.TableName);
        
        For<Metadata.Metadata>()
            .Column(c => c.VersionId, cm => cm.WithName("version_id"))
            .Column(c => c.ObjectId, cm => cm.WithName("object_id"))
            .Column(c => c.ContentType, cm => cm.WithName("content_type"))
            .Column(c => c.SizeInBytes, cm => cm.WithName("size_in_byes"))
            .Column(c => c.IsDeleted, cm => cm.WithName("is_deleted"))
            .Column(c => c.CustomMetadata, cm => cm.WithName("custom_metadata"))
            .Column(c => c.Tags, cm => cm.WithName("tags"))
            .Column(c => c.Tags, cm => cm.WithName("searchable_keywords"))
            .PartitionKey(c => c.VersionId)
            .TableName(Metadata.Metadata.TableName);

        For<Object.Object>()
            .TableName(Object.Object.TableName)
            .Column(c => c.Id, cm => cm.WithName("id"))
            .Column(c => c.BucketId, cm => cm.WithName("bucket_id"))
            .Column(c => c.VersionId, cm => cm.WithName("version_id"))
            .Column(c => c.FileKey, cm => cm.WithName("file_key"))
            .Column(c => c.FileName, cm => cm.WithName("file_name"))
            .Column(c => c.UploadedAt, cm => cm.WithName("uploaded_at"))
            .Column(c => c.UploadStatus, cm => cm.WithName("upload_status"))
            .Column(c => c.Path, cm => cm.WithName("path"))
            .PartitionKey(c => c.FileKey);
    }
}
