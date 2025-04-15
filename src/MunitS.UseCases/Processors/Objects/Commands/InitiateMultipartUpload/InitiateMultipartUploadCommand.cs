using MediatR;
using MunitS.Protos;
namespace MunitS.UseCases.Processors.Objects.Commands.InitiateMultipartUpload;

public sealed record InitiateMultipartUploadCommand(InitiateMultipartUploadRequest Request) : IRequest<InitiateMultipartUploadResponse>;
