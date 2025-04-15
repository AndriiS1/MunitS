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
namespace MunitS.UseCases.Processors.Objects.Queries.GetPartUploadUrl;

public class GetPartUploadUrlQueryHandler(IOptions<StorageOptions> options,
    IBucketByIdRepository bucketByIdRepository,
    IObjectByBucketIdRepository objectByBucketIdRepository) : IRequestHandler<GetPartUploadUrQuery, GetPartUploadUrlResponse>
{
    private const int UrlValidityInMinutes = 60;
    public async Task<GetPartUploadUrlResponse> Handle(GetPartUploadUrQuery query, CancellationToken cancellationToken)
    {
        var bucket = await bucketByIdRepository.Get(Guid.Parse(query.Request.BucketId));

        if (bucket == null) throw new RpcException(new Status(StatusCode.NotFound, $"Bucket with name: {query.Request.BucketId} is not found."));

        var @object = await objectByBucketIdRepository.GetByUploadId(bucket.Id, Guid.Parse(query.Request.UploadId));

        if (@object == null) throw new RpcException(new Status(StatusCode.NotFound, "Object with name is not found."));

        var expirationUnix = DateTimeOffset.UtcNow.AddMinutes(UrlValidityInMinutes).ToUnixTimeSeconds();

        var dataToSign = SignatureRule.GetSignature(bucket.Id.ToString(), query.Request.UploadId, query.Request.PartNumber, expirationUnix);

        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(options.Value.SignatureSecret));
        var signatureBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(dataToSign));
        var signature = Convert.ToHexString(signatureBytes).ToLowerInvariant();

        var queryParams = $"?bucketId={bucket.Id}&partNumber={query.Request.PartNumber}&expiresAt={expirationUnix}&signature={signature}";

        var url = $"{options.Value.BaseUrl}/objects/upload/{query.Request.UploadId}/parts/{queryParams}";

        return new GetPartUploadUrlResponse
        {
            Url = url
        };
    }
}
