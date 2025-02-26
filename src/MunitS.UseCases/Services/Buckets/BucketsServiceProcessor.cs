using Grpc.Core;
using MediatR;
using MunitS.Protos;
using MunitS.UseCases.Services.Buckets.Commands.Create;
using MunitS.UseCases.Services.Buckets.Commands.Delete;
using MunitS.UseCases.Services.Buckets.Queries.GetBucket;
using MunitS.UseCases.Services.Buckets.Queries.GetBuckets;
namespace MunitS.UseCases.Services.Buckets;

public class BucketsServiceProcessor(IMediator mediator) : BucketsService.BucketsServiceBase
{
    public override async Task<BucketServiceStatusResponse> CreateBucket(CreateBucketRequest request, ServerCallContext context)
    {
        return await mediator.Send(new CreateBucketCommand(request));
    }

    public override async Task<BucketServiceStatusResponse> DeleteBucket(DeleteBucketRequest request, ServerCallContext context)
    {
        return await mediator.Send(new DeleteBucketCommand(request));
    }
    
    public override async Task<GetBucketResponse> GetBucket(GetBucketRequest request, ServerCallContext context)
    {
        return await mediator.Send(new GetBucketQuery(request));
    }

    public override async Task<GetBucketsResponse> GetBuckets(GetBucketsRequest request, ServerCallContext context)
    {
        return await mediator.Send(new GetBucketsQuery(request));
    }
}

