using MediatR;
using MunitS.Protos;
namespace MunitS.UseCases.Processors.Buckets.Commands.Create;

public sealed record CreateBucketCommand(CreateBucketRequest Request) : IRequest<CreateBucketResponse>;
