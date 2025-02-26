using Grpc.Core;
using MediatR;
using MunitS.Protos;
namespace MunitS.UseCases.Processors.Objects.Commands.Create;

public sealed record CreateObjectCommand(CreateObjectRequest Request) : IRequest<ObjectServiceStatusResponse>;
