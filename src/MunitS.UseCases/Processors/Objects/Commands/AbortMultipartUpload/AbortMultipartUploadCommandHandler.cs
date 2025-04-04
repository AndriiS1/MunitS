using MediatR;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByIdRepository;
using MunitS.Infrastructure.Data.Repositories.Division;
using MunitS.Protos;
using MunitS.UseCases.Processors.Objects.Services;
using MunitS.UseCases.Processors.Service.PathRetriever;
namespace MunitS.UseCases.Processors.Objects.Commands.AbortMultipartUpload;

public class AbortMultipartUploadCommandHandler(IObjectsBuilder objectsBuilder,
    IBucketByIdRepository bucketByIdRepository,
    IDivisionRepository divisionRepository,
    IPathRetriever pathRetriever) : IRequestHandler<AbortMultipartUploadCommand, ObjectServiceStatusResponse>
{
    public async Task<ObjectServiceStatusResponse> Handle(AbortMultipartUploadCommand command, CancellationToken cancellationToken)
    {

        return new ObjectServiceStatusResponse
        {
            Status = "Success"
        };
    }
}
