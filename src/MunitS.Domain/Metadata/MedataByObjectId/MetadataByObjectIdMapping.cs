using Cassandra.Mapping;
namespace MunitS.Domain.Metadata.MedataByObjectId;

public class MetadataByObjectIdMapping : Mappings
{
    private const string TableName = "metadata_by_object_id";

    public MetadataByObjectIdMapping()
    {
        For<MetadataByObjectId>()
            .PartitionKey(c => c.BucketId, c => c.ObjectId)
            .Column(c => c.BucketId, cm => cm.WithName("bucket_id"))
            .Column(c => c.VersionId, cm => cm.WithName("version_id"))
            .Column(c => c.ObjectId, cm => cm.WithName("object_id"))
            .Column(c => c.ContentType, cm => cm.WithName("content_type"))
            .Column(c => c.SizeInBytes, cm => cm.WithName("size_in_bytes"))
            .Column(c => c.CustomMetadata, cm => cm.WithName("custom_metadata"))
            .Column(c => c.Tags, cm => cm.WithName("tags"))
            .TableName(TableName);
    }
}
