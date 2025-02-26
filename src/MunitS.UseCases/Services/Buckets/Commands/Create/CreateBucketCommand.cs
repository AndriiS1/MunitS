using MediatR;
using MunitS.Protos;
namespace MunitS.UseCases.Services.Buckets.Commands.Create;

public sealed record CreateBucketCommand(CreateBucketRequest Request) : IRequest<BucketServiceStatusResponse>;
