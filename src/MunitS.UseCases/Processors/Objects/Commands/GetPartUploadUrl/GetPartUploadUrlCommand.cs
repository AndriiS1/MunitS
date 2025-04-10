using MediatR;
using MunitS.Protos;
namespace MunitS.UseCases.Processors.Objects.Commands.GetPartUploadUrl;

public sealed record GetPartUploadUrlCommand(GetPartUploadUrlRequest Request) : IRequest<GetPartUploadUrlResponse>;
