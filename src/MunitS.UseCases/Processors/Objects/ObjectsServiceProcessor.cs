using Grpc.Core;
using MediatR;
using MunitS.Protos;
using MunitS.UseCases.Processors.Objects.Commands.AbortMultipartUpload;
using MunitS.UseCases.Processors.Objects.Commands.CompleteMultipartUpload;
using MunitS.UseCases.Processors.Objects.Commands.InitiateMultipartUpload;
using MunitS.UseCases.Processors.Objects.Queries.GetObjects;
using MunitS.UseCases.Processors.Objects.Queries.GetPartUploadUrl;
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
        return await mediator.Send(new GetPartUploadUrQuery(request));
    }

    public override async Task<ObjectServiceStatusResponse> CompleteMultipartUpload(CompleteMultipartUploadRequest request, ServerCallContext context)
    {
        return await mediator.Send(new CompleteMultipartUploadCommand(request));
    }

    public override async Task<GetObjectsSuffixesResponse> GetObjectsByPrefix(GetObjectByPrefixRequest request, ServerCallContext context)
    {
        return await mediator.Send(new GetObjectsQuery(request));
    }
}
