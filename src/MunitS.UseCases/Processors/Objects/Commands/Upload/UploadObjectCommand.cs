using Grpc.Core;
using MediatR;
using MunitS.Protos;
namespace MunitS.UseCases.Processors.Objects.Commands.Upload;

public sealed record UploadObjectCommand(IAsyncStreamReader<UploadObjectRequest> RequestStream) : IRequest<ObjectServiceStatusResponse>;
