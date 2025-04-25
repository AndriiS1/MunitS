using MunitS.Domain.Object.ObjectByUploadId;
namespace MunitS.UseCases.Processors.Objects.Services.ObjectDeletionService;

public interface IObjectDeletionService
{
    Task DeleteOldestObjectVersion(string bucketName, ObjectByUploadId objectByUploadId);
}
