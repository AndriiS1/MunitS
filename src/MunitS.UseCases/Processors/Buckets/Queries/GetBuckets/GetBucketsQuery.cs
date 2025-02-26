using MediatR;
using MunitS.Protos;
namespace MunitS.UseCases.Processors.Buckets.Queries.GetBuckets;

public sealed record GetBucketsQuery(GetBucketsRequest Request) : IRequest<GetBucketsResponse>;
