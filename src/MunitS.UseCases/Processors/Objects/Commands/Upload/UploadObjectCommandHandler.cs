using System.Security.Cryptography;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Http;
using MunitS.Domain.Directory;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByIdRepository;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByBucketIdRepository;
using MunitS.UseCases.Processors.Service.PathRetriever;
using MunitS.UseCases.Processors.Service.PathRetriever.Dtos;
namespace MunitS.UseCases.Processors.Objects.Commands.Upload;

public class UploadObjectCommandHandler(IBucketByIdRepository bucketByIdRepository,
    IObjectByBucketIdRepository objectByBucketIdRepository,
    IPathRetriever pathRetriever) : IRequestHandler<UploadObjectCommand, IResult>
{
    public async Task<IResult> Handle(UploadObjectCommand command, CancellationToken cancellationToken)
    {
        var bucket = await bucketByIdRepository.Get(command.BucketId);

        if (bucket == null) throw new RpcException(new Status(StatusCode.NotFound, $"Bucket with name: {command.BucketId} is not found."));

        var @object = await objectByBucketIdRepository.GetByUploadId(command.BucketId, command.UploadId);

        if (@object == null) return Results.NotFound("Object is not found.");

        var objectDirectories = new ObjectDirectories(bucket.Name, @object);
        var partPath = new PartPath(objectDirectories.TempObjectVersionDirectory, command.PartNumber);
        var absolutePartPath = pathRetriever.GetAbsoluteDirectoryPath(partPath);

        await using var stream = File.Create(absolutePartPath);

        await command.PartData.CopyToAsync(stream, cancellationToken);

        stream.Position = 0;

        await using var md5Stream = File.OpenRead(absolutePartPath);

        using var md5 = MD5.Create();
        var hash = await md5.ComputeHashAsync(md5Stream, cancellationToken);
        var etag = Convert.ToHexString(hash).ToLowerInvariant();

        return Results.Ok(new
        {
            ETag = etag
        });
    }
}
