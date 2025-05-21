using System.Security.Cryptography;
using MediatR;
using Microsoft.AspNetCore.Http;
using MunitS.Domain.Directory;
using MunitS.Domain.Directory.Dtos;
using MunitS.Domain.Metric.MetricByDate;
using MunitS.Domain.Part.PartByUploadId;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByIdRepository;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketCounter;
using MunitS.Infrastructure.Data.Repositories.Metric.MetricByDate;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByUploadIdRepository;
using MunitS.Infrastructure.Data.Repositories.Part.PartByUploadId;
using MunitS.UseCases.Processors.Service.PathRetriever;
namespace MunitS.UseCases.Processors.Objects.Commands.UploadPart;

public class UploadPartCommandHandler(IBucketByIdRepository bucketByIdRepository,
    IObjectByUploadIdRepository objectByUploadIdRepository,
    IPartByUploadIdRepository partByUploadIdRepository,
    IPathRetriever pathRetriever,
    IMetricByDateRepository metricByDateRepository,
    IBucketCounterRepository bucketCounterRepository) : IRequestHandler<UploadPartCommand, IResult>
{
    public async Task<IResult> Handle(UploadPartCommand command, CancellationToken cancellationToken)
    {
        var bucket = await bucketByIdRepository.Get(command.BucketId);

        if (bucket == null) return Results.NotFound($"Bucket with name: {command.BucketId} is not found.");

        var @object = await objectByUploadIdRepository.GetByUploadId(command.BucketId, command.ObjectId, command.UploadId);

        if (@object == null) return Results.NotFound("Object is not found.");

        var tryGetPart = await partByUploadIdRepository.Get(bucket.Id, command.UploadId, command.PartNumber);

        if (tryGetPart is not null) return Results.Conflict("Part with this number is already uploaded.");

        var objectDirectories = new ObjectVersionDirectories(bucket.Name, @object);
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

        await Task.WhenAll(partByUploadIdRepository.Create(partByUploadId),
            metricByDateRepository.Create(MetricByDate.Create(bucket.Id, Operation.UploadPart)),
            bucketCounterRepository.IncrementTypeAOperationsCount(bucket.Id));

        return Results.Ok(new
        {
            ETag = etag
        });
    }
}
