using MunitS.Domain.Directory;
namespace MunitS.UseCases.Processors.Service.PathRetriever;

public interface IPathRetriever
{
    string GetAbsoluteBucketDirectory(BucketDirectory bucketDirectory);
    string GetAbsoluteDivisionDirectory(DivisionDirectory divisionDirectory);
    string GetAbsoluteObjectTempVersionDirectory(TempObjectVersionDirectory objectDirectory);
    string GetAbsoluteObjectVersionDirectory(string objectPath, Guid versionId);
    string GetAbsoluteObjectDirectory(string objectPath);
}
