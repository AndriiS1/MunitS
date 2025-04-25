using MunitS.Domain.Division.DivisionByBucketId;
using MunitS.Domain.Object.ObjectByUploadId;
namespace MunitS.Domain.Directory.Dtos;

public class ObjectVersionDirectories
{
    public ObjectVersionDirectories(string bucketName, ObjectByUploadId objectByUploadId)
    {
        BucketDirectory = new BucketDirectory(bucketName);
        DivisionDirectory = new DivisionDirectory(bucketName, objectByUploadId.DivisionId, Enum.Parse<DivisionType.SizeType>(objectByUploadId.DivisionSizeType));
        ObjectDirectory = new ObjectDirectory(DivisionDirectory, objectByUploadId.Id);
        ObjectVersionDirectory = new ObjectVersionDirectory(ObjectDirectory, objectByUploadId.UploadId);
        TempObjectVersionDirectory = new TempObjectVersionDirectory(ObjectVersionDirectory);
    }

    public BucketDirectory BucketDirectory { get; }
    private DivisionDirectory DivisionDirectory { get; }
    public ObjectDirectory ObjectDirectory { get; }
    public ObjectVersionDirectory ObjectVersionDirectory { get; }
    public TempObjectVersionDirectory TempObjectVersionDirectory { get; }
}
