using MediatR;
using MunitS.Protos;
namespace MunitS.UseCases.Processors.Objects.Queries.GetPartUploadUrl;

public sealed record GetPartUploadUrQuery(GetPartUploadUrlRequest Request) : IRequest<GetPartUploadUrlResponse>;
