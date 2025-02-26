using Grpc.Core;
using MediatR;
using MunitS.Protos;
using MunitS.UseCases.Services.Objects.Commands;
namespace MunitS.UseCases.Services.Objects;

public class ObjectsServiceProcessor(IMediator mediator) : ObjectsService.ObjectsServiceBase
{
    public override async Task<ObjectServiceStatusResponse> UploadObject(UploadObjectRequest request, ServerCallContext context)
    {
        return await mediator.Send(new UploadFileCommand(request, context));
    }
}

