using MediatR;
using MunitS.Protos;
namespace MunitS.UseCases.Processors.Buckets.Queries.BucketExists;

public sealed record BucketExistsQuery(BucketExistsCheckRequest Request) : IRequest<BucketExistsResponse>;
