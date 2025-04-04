using MediatR;
using MunitS.Protos;
namespace MunitS.UseCases.Processors.Objects.Commands.CompleteMultipartUpload;

public sealed record CompleteMultipartUploadCommand(CompleteMultipartUploadRequest Request) : IRequest<ObjectServiceStatusResponse>;
