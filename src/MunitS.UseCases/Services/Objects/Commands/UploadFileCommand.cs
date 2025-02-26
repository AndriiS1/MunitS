using Grpc.Core;
using MediatR;
using MunitS.Protos;
namespace MunitS.UseCases.Services.Objects.Commands;

public sealed record UploadFileCommand(UploadObjectRequest Request, ServerCallContext Context) : IRequest<ObjectServiceStatusResponse>;
