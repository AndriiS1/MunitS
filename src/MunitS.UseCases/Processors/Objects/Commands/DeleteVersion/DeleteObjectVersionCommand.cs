using MediatR;
using MunitS.Protos;
namespace MunitS.UseCases.Processors.Objects.Commands.DeleteVersion;

public sealed record DeleteObjectVersionCommand(DeleteObjectVersionRequest Request) : IRequest<ObjectServiceStatusResponse>;
