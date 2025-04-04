using MediatR;
using MunitS.Protos;
namespace MunitS.UseCases.Processors.Objects.Commands.AbortMultipartUpload;

public sealed record AbortMultipartUploadCommand(AbortMultipartUploadRequest Request) : IRequest<ObjectServiceStatusResponse>;
