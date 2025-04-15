using MunitS.Domain.Division.DivisionByBucketId;
using MunitS.Domain.Object.ObjectByBucketId;
namespace MunitS.Domain.Directory.Dtos;

public class ObjectVersionDirectories
{
    public ObjectVersionDirectories(string bucketName, ObjectByBucketId objectByBucketId)
    {
        BucketDirectory = new BucketDirectory(bucketName);
        DivisionDirectory = new DivisionDirectory(bucketName, objectByBucketId.DivisionId, objectByBucketId.DivisionSizeType);
        ObjectDirectory = new ObjectDirectory(DivisionDirectory, objectByBucketId.Id);
        ObjectVersionDirectory = new ObjectVersionDirectory(ObjectDirectory, objectByBucketId.UploadId);
        TempObjectVersionDirectory = new TempObjectVersionDirectory(ObjectVersionDirectory);
    }

    public BucketDirectory BucketDirectory { get; }
    private DivisionDirectory DivisionDirectory { get; }
    public ObjectDirectory ObjectDirectory { get; }
    public ObjectVersionDirectory ObjectVersionDirectory { get; }
    public TempObjectVersionDirectory TempObjectVersionDirectory { get; }
}
