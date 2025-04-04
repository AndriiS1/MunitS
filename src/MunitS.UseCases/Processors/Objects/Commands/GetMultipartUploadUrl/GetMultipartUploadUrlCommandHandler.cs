using Grpc.Core;
using MediatR;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByIdRepository;
using MunitS.Infrastructure.Data.Repositories.Division;
using MunitS.Protos;
using MunitS.UseCases.Processors.Objects.Services;
using MunitS.UseCases.Processors.Service.PathRetriever;
namespace MunitS.UseCases.Processors.Objects.Commands.GetMultipartUploadUrl;

public class GetMultipartUploadUrlCommandHandler(IObjectsBuilder objectsBuilder,
    IBucketByIdRepository bucketByIdRepository,
    IDivisionRepository divisionRepository,
    IPathRetriever pathRetriever) : IRequestHandler<GetMultipartUploadUrlCommand, GetMultipartUploadUrlResponse>
{
    public async Task<GetMultipartUploadUrlResponse> Handle(GetMultipartUploadUrlCommand command, CancellationToken cancellationToken)
    {
        var bucket = await bucketByIdRepository.Get(Guid.Parse(command.Request.BucketId));

        if (bucket == null) throw new RpcException(new Status(StatusCode.NotFound, $"Bucket with name: {command.Request.BucketId} is not found."));

  
        return new GetMultipartUploadUrlResponse
        {
            Url = ""
        };
    }
}
