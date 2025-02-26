using MediatR;
using MunitS.Protos;
namespace MunitS.UseCases.Services.Buckets.Queries.GetBucket;

public sealed record GetBucketQuery(GetBucketRequest Request) : IRequest<GetBucketResponse>;
