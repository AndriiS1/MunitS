using MediatR;
using Microsoft.AspNetCore.Mvc;
using MunitS.UseCases.Processors.Objects.Commands.Upload;
namespace MunitS.Apis.Objects;

public static class ObjectsEndpoints
{
    private const string Source = "ObjectsApi";

    public static void MapObjectsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPut("objects/upload/{uploadId}/parts", async ([FromRoute] string uploadId, [FromQuery] string bucketId,
                [FromQuery] int partNumber, [FromBody] IFormFile file,
                [FromServices] IMediator mediator) => await mediator.Send(new UploadObjectCommand(Guid.Parse(bucketId),
                Guid.Parse(uploadId), file, partNumber)))
            .WithGroupName(Source)
            .DisableAntiforgery()
            .RequireAuthorization();
    }
}
