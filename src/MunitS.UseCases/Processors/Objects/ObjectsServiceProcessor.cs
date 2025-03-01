using Grpc.Core;
using MediatR;
using MunitS.Protos;
using MunitS.UseCases.Processors.Objects.Commands.Create;
using MunitS.UseCases.Processors.Objects.Commands.Upload;
namespace MunitS.UseCases.Processors.Objects;

public class ObjectsServiceProcessor(IMediator mediator) : ObjectsService.ObjectsServiceBase
{
    public override async Task<ObjectServiceStatusResponse> CreateObject(CreateObjectRequest request, ServerCallContext context)
    {
        return await mediator.Send(new CreateObjectCommand(request));
    }
    
    public override async Task<ObjectServiceStatusResponse> UploadObject(IAsyncStreamReader<UploadObjectRequest> requestStream, ServerCallContext context)
    {
        return await mediator.Send(new UploadObjectCommand(requestStream));
    }
}

