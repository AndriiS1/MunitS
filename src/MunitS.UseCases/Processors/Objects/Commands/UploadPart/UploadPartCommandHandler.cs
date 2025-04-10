using System.Security.Cryptography;
using MediatR;
using Microsoft.AspNetCore.Http;
using MunitS.Domain.Directory;
using MunitS.Domain.Part.PartByUploadId;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByIdRepository;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByBucketIdRepository;
using MunitS.Infrastructure.Data.Repositories.Part.PartByUploadId;
using MunitS.UseCases.Processors.Service.PathRetriever;
using MunitS.UseCases.Processors.Service.PathRetriever.Dtos;
namespace MunitS.UseCases.Processors.Objects.Commands.UploadPart;

public class UploadPartCommandHandler(IBucketByIdRepository bucketByIdRepository,
    IObjectByBucketIdRepository objectByBucketIdRepository,
    IPartByUploadIdRepository partByUploadIdRepository,
    IPathRetriever pathRetriever) : IRequestHandler<UploadPartCommand, IResult>
{
    public async Task<IResult> Handle(UploadPartCommand command, CancellationToken cancellationToken)
    {
        var bucket = await bucketByIdRepository.Get(command.BucketId);

        if (bucket == null) return Results.NotFound($"Bucket with name: {command.BucketId} is not found.");

        var @object = await objectByBucketIdRepository.GetByUploadId(command.BucketId, command.UploadId);

        if (@object == null) return Results.NotFound("Object is not found.");

        var tryGetPart = await partByUploadIdRepository.Get(bucket.Id, command.UploadId, command.PartNumber);

        if (tryGetPart is not null) return Results.Conflict("Part with this number is already uploaded.");

        var objectDirectories = new ObjectDirectories(bucket.Name, @object);
        var partPath = new PartPath(objectDirectories.TempObjectVersionDirectory, command.PartNumber);
        var absolutePartPath = pathRetriever.GetAbsoluteDirectoryPath(partPath);

        await using var stream = File.Create(absolutePartPath);

        using var md5 = MD5.Create();
        
        await using (var cryptoStream = new CryptoStream(stream, md5, CryptoStreamMode.Write))
        {
            await command.PartData.CopyToAsync(cryptoStream, cancellationToken);
        }
        
        var hash = md5.Hash;
        var etag = Convert.ToHexString(hash!).ToLowerInvariant();

        var partByUploadId = PartByUploadId.Create(bucket.Id, command.UploadId, etag, command.PartNumber);
        await partByUploadIdRepository.Create(partByUploadId);

        return Results.Ok(new
        {
            ETag = etag
        });
    }
}
