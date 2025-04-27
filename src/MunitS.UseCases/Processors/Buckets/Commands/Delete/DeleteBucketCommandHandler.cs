using Grpc.Core;
using MediatR;
using MunitS.Domain.Directory;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByIdRepository;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByNameRepository;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketCounter;
using MunitS.Infrastructure.Data.Repositories.Division.DivisionById;
using MunitS.Infrastructure.Data.Repositories.Division.DivisionCounters;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByFileKeyRepository;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByUploadIdRepository;
using MunitS.Infrastructure.Data.Repositories.ObjectSuffix.ObjectSuffixByParentPrefixRepository;
using MunitS.Protos;
using MunitS.UseCases.Processors.Service.PathRetriever;
namespace MunitS.UseCases.Processors.Buckets.Commands.Delete;

public class UploadFileCommandHandler(IBucketByIdRepository bucketByIdRepository,
    IBucketByNameRepository bucketByNameRepository,
    IPathRetriever pathRetriever,
    IBucketCounterRepository bucketBucketCounterRepository,
    IDivisionByIdRepository divisionByIdRepository,
    IDivisionCounterRepository divisionCounterRepository,
    IObjectByUploadIdRepository objectByUploadIdRepository,
    IObjectByFileKeyRepository objectByFileKeyRepository,
    IObjectSuffixByParentPrefixRepository objectSuffixByParentPrefixRepository
) : IRequestHandler<DeleteBucketCommand, BucketServiceStatusResponse>
{
    public async Task<BucketServiceStatusResponse> Handle(DeleteBucketCommand command, CancellationToken cancellationToken)
    {
        var bucket = await bucketByIdRepository.Get(new Guid(command.Request.Id));

        if (bucket == null)
        {
            throw new RpcException(
                new Status(StatusCode.NotFound, $"Bucket {command.Request.Id} does not exists.")
            );
        }

        var bucketDirectory = new BucketDirectory(bucket.Name);
        var absoluteBucketDirectory = pathRetriever.GetAbsoluteDirectoryPath(bucketDirectory);

        await Task.WhenAll(bucketByIdRepository.Delete(bucket.Id), bucketByNameRepository.Delete(bucket.Name), bucketBucketCounterRepository.Delete(bucket.Id), divisionByIdRepository.Delete(bucket.Id), divisionCounterRepository.Delete(bucket.Id),
            objectByUploadIdRepository.Delete(bucket.Id), objectByFileKeyRepository.Delete(bucket.Id), objectSuffixByParentPrefixRepository.Delete(bucket.Id));

        Directory.Delete(absoluteBucketDirectory, true);

        return new BucketServiceStatusResponse
        {
            Status = "Success"
        };
    }
}
