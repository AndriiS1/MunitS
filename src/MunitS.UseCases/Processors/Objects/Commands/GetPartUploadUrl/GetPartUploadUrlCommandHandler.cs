using System.Security.Cryptography;
using System.Text;
using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Options;
using MunitS.Domain.Rules;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByIdRepository;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByBucketIdRepository;
using MunitS.Infrastructure.Options.Storage;
using MunitS.Protos;
namespace MunitS.UseCases.Processors.Objects.Commands.GetPartUploadUrl;

public class GetPartUploadUrlCommandHandler(IOptions<StorageOptions> options,
    IBucketByIdRepository bucketByIdRepository,
    IObjectByBucketIdRepository objectByBucketIdRepository) : IRequestHandler<GetPartUploadUrlCommand, GetPartUploadUrlResponse>
{
    private const int UrlValidityInMinutes = 60;
    public async Task<GetPartUploadUrlResponse> Handle(GetPartUploadUrlCommand command, CancellationToken cancellationToken)
    {
        var bucket = await bucketByIdRepository.Get(Guid.Parse(command.Request.BucketId));

        if (bucket == null) throw new RpcException(new Status(StatusCode.NotFound, $"Bucket with name: {command.Request.BucketId} is not found."));

        var @object = await objectByBucketIdRepository.GetByUploadId(bucket.Id, Guid.Parse(command.Request.BucketId));

        if (@object == null) throw new RpcException(new Status(StatusCode.NotFound, "Object with name is not found."));

        var expirationUnix = DateTimeOffset.UtcNow.AddMinutes(UrlValidityInMinutes).ToUnixTimeSeconds();

        var dataToSign = SignatureRule.GetSignature(bucket.Id.ToString(), command.Request.UploadId, command.Request.PartNumber, expirationUnix);

        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(options.Value.SignatureSecret));
        var signatureBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(dataToSign));
        var signature = Convert.ToHexString(signatureBytes).ToLowerInvariant();
        
        var query = $"?bucketId={bucket.Id}&partNumber={command.Request.PartNumber}&expiresAt={expirationUnix}&signature={signature}";

        var url = $"{options.Value.BaseUrl}/objects/upload/{command.Request.UploadId}/parts/{query}";

        return new GetPartUploadUrlResponse
        {
            Url = url
        };
    }
}
