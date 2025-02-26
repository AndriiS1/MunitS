using Grpc.Core;
using MediatR;
using MunitS.Protos;
using MunitS.UseCases.Commands;
namespace MunitS.UseCases.Services.Object;

public class ObjectsServiceProcessor(IMediator mediator) : Protos.ObjectsService.ObjectsServiceBase
{
    public override async Task<NoContentResponse> UploadObject(UploadObjectRequest request, ServerCallContext context)
    {
        return await mediator.Send(new UploadFileCommand(request, context));
    }
}

