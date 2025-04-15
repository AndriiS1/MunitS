using Grpc.Core;
using MediatR;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByIdRepository;
using MunitS.Infrastructure.Data.Repositories.Division.DivisionById;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByFileKeyRepository;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByUploadIdRepository;
using MunitS.Protos;
using MunitS.UseCases.Processors.Objects.Services.DivisionBuilder;
using MunitS.UseCases.Processors.Objects.Services.ObjectBuilder;
using MunitS.UseCases.Processors.Service.PathRetriever;
namespace MunitS.UseCases.Processors.Objects.Commands.Delete;

public class DeleteObjectCommandHandler(IObjectsBuilder objectsBuilder,
    IObjectByFileKeyRepository objectByFileKeyRepository,
    IBucketByIdRepository bucketByIdRepository,
    IObjectByUploadIdRepository objectByUploadIdRepository,
    IPathRetriever pathRetriever,
    IDivisionByIdRepository divisionByIdRepository,
    IDivisionBuilder divisionBuilder) : IRequestHandler<DeleteObjectCommand, InitiateMultipartUploadResponse>
{
    public async Task<InitiateMultipartUploadResponse> Handle(DeleteObjectCommand command, CancellationToken cancellationToken)
    {
        var bucket = await bucketByIdRepository.Get(Guid.Parse(command.Request.BucketId));

        if (bucket == null) throw new RpcException(new Status(StatusCode.NotFound, $"Bucket with name: {command.Request.BucketId} is not found."));

        // var objects = await objectByFileKeyRepository.GetAll(bucket.Id, command.Request.FileKey);
        //
        // if (objects.Count == 0)
        // {
        //     throw new RpcException(new Status(StatusCode.NotFound, "No object versions found."));
        // }

        // var objectVersions = await objectByUploadIdRepository.GetAll(bucket.Id, objects.Select(o => o.UploadId));

        return new InitiateMultipartUploadResponse();
    }
}
