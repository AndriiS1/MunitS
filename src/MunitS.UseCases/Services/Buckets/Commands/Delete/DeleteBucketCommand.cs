using MediatR;
using MunitS.Protos;
namespace MunitS.UseCases.Services.Buckets.Commands.Delete;

public sealed record DeleteBucketCommand(DeleteBucketRequest Request) : IRequest<BucketServiceStatusResponse>;
