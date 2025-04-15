using MediatR;
using MunitS.Protos;
namespace MunitS.UseCases.Processors.Objects.Queries.GetObjects;

public sealed record GetObjectsQuery(GetObjectByPrefixRequest Request) : IRequest<GetObjectsSuffixesResponse>;
