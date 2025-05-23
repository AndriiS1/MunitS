using Grpc.Core;
using MediatR;
using MunitS.Protos;
using MunitS.UseCases.Processors.Buckets.Commands.Create;
using MunitS.UseCases.Processors.Buckets.Commands.Delete;
using MunitS.UseCases.Processors.Buckets.Queries.BucketExists;
using MunitS.UseCases.Processors.Buckets.Queries.GetBucket;
using MunitS.UseCases.Processors.Buckets.Queries.GetBuckets;
namespace MunitS.UseCases.Processors.Buckets;

public class BucketsServiceProcessor(IMediator mediator) : BucketsService.BucketsServiceBase
{
    public override async Task<CreateBucketResponse> CreateBucket(CreateBucketRequest request, ServerCallContext context)
    {
        return await mediator.Send(new CreateBucketCommand(request));
    }

    public override async Task<BucketServiceStatusResponse> DeleteBucket(DeleteBucketRequest request, ServerCallContext context)
    {
        return await mediator.Send(new DeleteBucketCommand(request));
    }

    public override async Task<BucketResponse> GetBucket(GetBucketRequest request, ServerCallContext context)
    {
        return await mediator.Send(new GetBucketQuery(request));
    }

    public override async Task<BucketExistsResponse> BucketExistsCheck(BucketExistsCheckRequest request, ServerCallContext context)
    {
        return await mediator.Send(new BucketExistsQuery(request));
    }

    public override async Task<GetBucketsResponse> GetBuckets(GetBucketsRequest request, ServerCallContext context)
    {
        return await mediator.Send(new GetBucketsQuery(request));
    }

    public override async Task<GetMetricsResponse> GetBucketMetrics(GetMetricsRequest request, ServerCallContext context)
    {
        return await mediator.Send(new GetMetricsQuery(request));
    }
}
