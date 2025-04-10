using Grpc.Core;
using MediatR;
using MunitS.Protos;
using MunitS.UseCases.Processors.Objects.Commands.AbortMultipartUpload;
using MunitS.UseCases.Processors.Objects.Commands.CompleteMultipartUpload;
using MunitS.UseCases.Processors.Objects.Commands.GetPartUploadUrl;
using MunitS.UseCases.Processors.Objects.Commands.InitiateMultipartUpload;
using MunitS.UseCases.Processors.Objects.Queries.GetObjects;
namespace MunitS.UseCases.Processors.Objects;

public class ObjectsServiceProcessor(IMediator mediator) : ObjectsService.ObjectsServiceBase
{
    public override async Task<InitiateMultipartUploadResponse> InitiateMultipartUpload(InitiateMultipartUploadRequest request, ServerCallContext context)
    {
        return await mediator.Send(new InitiateMultipartUploadCommand(request));
    }

    public override async Task<ObjectServiceStatusResponse> AbortMultipartUpload(AbortMultipartUploadRequest request, ServerCallContext context)
    {
        return await mediator.Send(new AbortMultipartUploadCommand(request));
    }

    public override async Task<GetPartUploadUrlResponse> GetPartUploadUrl(GetPartUploadUrlRequest request, ServerCallContext context)
    {
        return await mediator.Send(new GetPartUploadUrlCommand(request));
    }

    public override async Task<ObjectServiceStatusResponse> CompleteMultipartUpload(CompleteMultipartUploadRequest request, ServerCallContext context)
    {
        return await mediator.Send(new CompleteMultipartUploadCommand(request));
    }

    public override async Task<GetObjectsByPrefixResponse> GetObjectByPrefix(GetObjectByPrefixRequest request, ServerCallContext context)
    {
        return await mediator.Send(new GetObjectsQuery(request));
    }
}
