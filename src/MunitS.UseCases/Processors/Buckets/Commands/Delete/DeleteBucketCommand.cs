using MediatR;
using MunitS.Protos;
namespace MunitS.UseCases.Processors.Buckets.Commands.Delete;

public sealed record DeleteBucketCommand(DeleteBucketRequest Request) : IRequest<BucketServiceStatusResponse>;
