using MunitS.Domain.Directory;
using MunitS.Domain.Division.DivisionByBucketId;
using MunitS.Domain.Object.ObjectByBucketId;
namespace MunitS.UseCases.Processors.Service.PathRetriever.Dtos;

public class ObjectDirectories
{

    public ObjectDirectories(string bucketName, ObjectByBucketId objectByBucketId)
    {
        var divisionSizeType = Enum.Parse<DivisionType.SizeType>(objectByBucketId.DivisionSizeType);

        BucketDirectory = new BucketDirectory(bucketName);
        DivisionDirectory = new DivisionDirectory(bucketName, objectByBucketId.DivisionId, divisionSizeType);
        ObjectDirectory = new ObjectDirectory(DivisionDirectory, objectByBucketId.Id);
        ObjectVersionDirectory = new ObjectVersionDirectory(ObjectDirectory, objectByBucketId.UploadId);
        TempObjectVersionDirectory = new TempObjectVersionDirectory(ObjectVersionDirectory);
    }
    public BucketDirectory BucketDirectory { get; }
    public DivisionDirectory DivisionDirectory { get; }
    public ObjectDirectory ObjectDirectory { get; }
    public ObjectVersionDirectory ObjectVersionDirectory { get; }
    public TempObjectVersionDirectory TempObjectVersionDirectory { get; }
}
