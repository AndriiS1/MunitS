using MediatR;
using MunitS.Protos;
namespace MunitS.UseCases.Processors.Objects.Commands.Delete;

public sealed record DeleteObjectCommand(DeleteObjectRequest Request) : IRequest<InitiateMultipartUploadResponse>;
