using MediatR;
using MunitS.Protos;
namespace MunitS.UseCases.Processors.Objects.Queries.GetObject;

public sealed record GetObjectQuery(GetObjectRequest Request) : IRequest<GetObjectResponse>;
