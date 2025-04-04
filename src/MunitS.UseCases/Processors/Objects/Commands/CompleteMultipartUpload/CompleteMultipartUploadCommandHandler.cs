using MediatR;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByIdRepository;
using MunitS.Infrastructure.Data.Repositories.Division;
using MunitS.Protos;
using MunitS.UseCases.Processors.Objects.Services;
using MunitS.UseCases.Processors.Service.PathRetriever;
namespace MunitS.UseCases.Processors.Objects.Commands.CompleteMultipartUpload;

public class CompleteMultipartUploadCommandHandler(IObjectsBuilder objectsBuilder,
    IBucketByIdRepository bucketByIdRepository,
    IDivisionRepository divisionRepository,
    IPathRetriever pathRetriever) : IRequestHandler<CompleteMultipartUploadCommand, ObjectServiceStatusResponse>
{
    public async Task<ObjectServiceStatusResponse> Handle(CompleteMultipartUploadCommand command, CancellationToken cancellationToken)
    {

        return new ObjectServiceStatusResponse
        {
            Status = "Success"
        };
    }
    
}
