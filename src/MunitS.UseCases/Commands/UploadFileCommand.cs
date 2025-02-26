using Grpc.Core;
using MediatR;
using MunitS.Protos;
namespace MunitS.UseCases.Commands;

public sealed record UploadFileCommand(UploadObjectRequest Request, ServerCallContext Context) : IRequest<NoContentResponse>;
