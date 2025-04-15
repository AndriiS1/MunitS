using System.Security.Cryptography;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MunitS.Domain.Rules;
using MunitS.Infrastructure.Options.Storage;
using MunitS.UseCases.Processors.Objects.Commands.UploadPart;
namespace MunitS.Apis.Objects;

public static class ObjectsEndpoints
{
    private const string Source = "ObjectsApi";

    public static void MapObjectsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPut("objects/upload/{uploadId}/parts", UploadObject)
            .WithGroupName(Source)
            .DisableAntiforgery();
    }

    private static async Task<IResult> UploadObject([FromRoute] string uploadId, [FromQuery] string bucketId, [FromQuery] string objectId,
        [FromQuery] int partNumber, [FromQuery] long expiresAt, [FromQuery] string signature, [FromForm] IFormFile file,
        [FromServices] IMediator mediator, [FromServices] IOptions<StorageOptions> options)
    {
        if (!ValidateSignedUrl(uploadId, bucketId, objectId, partNumber, expiresAt, signature, options.Value.SignatureSecret))
        {
            return Results.Forbid();
        }

        return await mediator.Send(new UploadPartCommand(Guid.Parse(bucketId), Guid.Parse(objectId),
            Guid.Parse(uploadId), file, partNumber));
    }

    private static bool ValidateSignedUrl(string uploadId, string bucketId, string objectId, int partNumber, long expiresAt, string signature, string signatureSecret)
    {
        var nowUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        if (nowUnix > expiresAt) return false;

        var dataToSign = SignatureRule.GetSignature(bucketId, objectId, uploadId, partNumber, expiresAt);

        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(signatureSecret));
        var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(dataToSign));
        var expectedSignature = Convert.ToHexString(hashBytes).ToLowerInvariant();

        return signature == expectedSignature;
    }
}
