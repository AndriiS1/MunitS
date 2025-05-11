using MediatR;
using MunitS.Protos;

public sealed record GetMetricsQuery(GetMetricsRequest Request) : IRequest<GetMetricsResponse>;
