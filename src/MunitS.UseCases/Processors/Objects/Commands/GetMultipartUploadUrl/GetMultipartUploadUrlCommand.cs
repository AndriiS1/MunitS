using MediatR;
using MunitS.Protos;
namespace MunitS.UseCases.Processors.Objects.Commands.GetMultipartUploadUrl;

public sealed record GetMultipartUploadUrlCommand(GetMultipartUploadUrlRequest Request) : IRequest<GetMultipartUploadUrlResponse>;
