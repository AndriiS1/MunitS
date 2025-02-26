using MediatR;
using MunitS.Protos;
namespace MunitS.UseCases.Processors.Buckets.Queries.GetBucket;

public sealed record GetBucketQuery(GetBucketRequest Request) : IRequest<GetBucketResponse>;
